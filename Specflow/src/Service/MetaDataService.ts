import { IWebPartContext } from '@microsoft/sp-webpart-base';
import { ICache } from '@asia/asia-lib/lib/framework/cache/ICache';
import { LocalStorageCache } from '@asia/asia-lib/lib/framework/cache/LocalStorageCache';
import { Utilities, Intervals } from '@asia/asia-lib/lib/framework/Utilities';
import { UserProfileService, IUserProfile } from "@asia/asia-lib/lib/core";
import { Version, Environment, EnvironmentType } from '@microsoft/sp-core-library';
import {
  SPHttpClient, SPHttpClientConfiguration, SPHttpClientResponse, ODataVersion, ISPHttpClientConfiguration, ISPHttpClientOptions,
  ISPHttpClientBatchOptions, SPHttpClientBatch, ISPHttpClientBatchCreationOptions
} from '@microsoft/sp-http';

export interface ICurrentUserInfo {
  Id: number;
  Title: string;
  LoginName: string;
  Email: string;
  IsSPMO: boolean;
  ADDepartment: string;
  PPPDepartment: string;
}
export interface IUserInfo {
  Id: number;
  Title: string;
  LoginName: string;
  Email: string;
}
export interface IOption {
  ID: number;
  Title: string;
}
export interface IScoredOption {
  ID: number;
  Title: string;
  TypeAScore: number;//PPP_Score
  TypeBScore: number;
}
export interface IDeptMapping {
  ID: number;
  Title: string;
  PPP_Department: string;
}
export interface IPorjectType {
  ID: number;
  Title: string;
  QueueName: string;
  QueueNumber: string;
  PrioritizedMatrix: string;
  ProjectCategory: string;
}

export class IPPMMetaDataContext {
  public currentUser: ICurrentUserInfo;
  public listNames: string[];
  public deptMappings: IDeptMapping[];
  public enhanceSatificationTypes: IScoredOption[];
  public improvedKPITypes: IScoredOption[];
  public riskMitigatingTypes: IScoredOption[];
  public projectTypes: IPorjectType[];
  public SiteUsers: IUserInfo[];
}

declare class IUserProfileProperty {
  public Key: string;
  public Value: string;
  public ValueType: string;
}
export interface IMetaDataService {
  getMetaData(): Promise<IPPMMetaDataContext>;
}

const CACHE_NAME_DeptMappings = `META_DeptMappings`;
const CACHE_NAME_Lists = 'META_Lists';
export class MetaDataService implements IMetaDataService {
  private static _cache: ICache = new LocalStorageCache();
  private _cachePrefix: string;
  private context: IWebPartContext;
  private _webAbsoluteUrl: string;
  private spHttpClient: SPHttpClient;

  public constructor(webPartContext: IWebPartContext) {
    this.context = webPartContext;
    this._cachePrefix = webPartContext.pageContext.web.serverRelativeUrl.replace('/sites/', '');

    this._webAbsoluteUrl = webPartContext.pageContext.web.absoluteUrl;
    this.spHttpClient = webPartContext.spHttpClient;
  }
  public getMetaData(): Promise<IPPMMetaDataContext> {
    const CACHE_NAME_MetaDataContexts: string = 'CACHE_NAME_MetaDataContexts';
    const cache = MetaDataService._cache;
    const cachePrefix = this._cachePrefix;
    const webAbsoluteUrl = this._webAbsoluteUrl;
    return new Promise<IPPMMetaDataContext>((resolve, reject) => {
      if (cache.exists(cachePrefix + '_' + CACHE_NAME_MetaDataContexts)) {
        console.info(`Found MetaDataContext in cache.`);
        resolve(JSON.parse(cache.get(cachePrefix + '_' + CACHE_NAME_MetaDataContexts)));
      } else {
        console.info(`MetaDataContext NOT Found in Cache.`);
        var metadataInstance: IPPMMetaDataContext = new IPPMMetaDataContext();
        let spBatchCreationOpts = { webUrl: webAbsoluteUrl };
        let spBatch = this.spHttpClient.beginBatch(spBatchCreationOpts);

        var getMyProperties: Promise<SPHttpClientResponse> = spBatch.get(
          webAbsoluteUrl + `/_api/SP.UserProfiles.PeopleManager/GetMyProperties`,
          SPHttpClientBatch.configurations.v1);
        var getCurrentUser: Promise<SPHttpClientResponse> = spBatch.get(
          webAbsoluteUrl + `/_api/Web/CurrentUser?$select=ID,Title,LoginName,EMail`,
          SPHttpClientBatch.configurations.v1);
        var getListNames: Promise<SPHttpClientResponse> = spBatch.get(
          webAbsoluteUrl + `/_api/Lists?$select=Title`,
          SPHttpClientBatch.configurations.v1);
        var getDeptMappings: Promise<SPHttpClientResponse> = spBatch.get(
          webAbsoluteUrl + `/_api/Lists/GetByTitle('MD_DeptMapping')/Items?$Select=ID,Title,PPP_Department&orderby=PPP_Order&filter=PPP_IsActive eq 1`,
          SPHttpClientBatch.configurations.v1);
        var getEnhanceSatificationTypes: Promise<SPHttpClientResponse> = spBatch.get(
          webAbsoluteUrl + `/_api/Lists/GetByTitle('MD_EnhanceSatificationType')/Items?$Select=ID,Title,PPP_Score,TypeBScore&orderby=PPP_Order&filter=PPP_IsActive eq 1`,
          SPHttpClientBatch.configurations.v1);
        var getImprovedKPITypes: Promise<SPHttpClientResponse> = spBatch.get(
          webAbsoluteUrl + `/_api/Lists/GetByTitle('MD_ImprovedKPIType')/Items?$Select=ID,Title,PPP_Score,TypeBScore&orderby=PPP_Order&filter=PPP_IsActive eq 1`,
          SPHttpClientBatch.configurations.v1);
        var getProjectTypes: Promise<SPHttpClientResponse> = spBatch.get(
          webAbsoluteUrl + `/_api/Lists/GetByTitle('MD_ProjectType')/Items?$Select=ID,Title,QueueName,QueueNumber,PrioritizedMatrix,ProjectCategory&orderby=PPP_Order&filter=PPP_IsActive eq 1`,
          SPHttpClientBatch.configurations.v1);
        var getRiskMitigatingTypes: Promise<SPHttpClientResponse> = spBatch.get(
          webAbsoluteUrl + `/_api/Lists/GetByTitle('MD_RiskMitigatingType')/Items?$Select=ID,Title,PPP_Score,TypeBScore&orderby=PPP_Order&filter=PPP_IsActive eq 1`,
          SPHttpClientBatch.configurations.v1);
        var getSiteUsers: Promise<SPHttpClientResponse> = spBatch.get(
          webAbsoluteUrl + `/_api/web/siteusers`,
          SPHttpClientBatch.configurations.v1);

        spBatch.execute().then(() => {
          console.info(`MetaDataContext: Get batch from server.`);
          var flag = 0;
          getMyProperties.then((response: SPHttpClientResponse) => {
            response.json().then((myProfile: IUserProfile) => {
              let properties: IUserProfileProperty[] = myProfile.UserProfileProperties.filter(prop => {
                return prop.Key === 'Department';
              });
              if (properties !== undefined && properties.length > 0) {
                if (!metadataInstance.currentUser) {
                  metadataInstance.currentUser = {} as ICurrentUserInfo;
                }
                properties.forEach(prop => {
                  if (prop.Key === 'Department') {
                    console.info(`MetaDataContext: Get ADDepartment from server.`);
                    metadataInstance.currentUser.ADDepartment = prop.Value;
                  }
                });
              }
              flag = flag | 1;
            });
          });

          getCurrentUser.then((response: SPHttpClientResponse) => {
            response.json().then((data: any) => {
              console.info(`MetaDataContext: Get currentUser from server.`);
              if (!metadataInstance.currentUser) {
                metadataInstance.currentUser = {} as ICurrentUserInfo;
              }
              metadataInstance.currentUser.Id = data.Id;
              metadataInstance.currentUser.LoginName = data.LoginName;
              metadataInstance.currentUser.Title = data.Title;
              metadataInstance.currentUser.Email = data.Email;
              flag = flag | 2;
            });
          });

          getListNames.then((response: SPHttpClientResponse) => {
            metadataInstance.listNames = [];
            response.json().then((data) => {
              if (!metadataInstance.currentUser) {
                metadataInstance.currentUser = {} as ICurrentUserInfo;
              }
              metadataInstance.currentUser.IsSPMO = false;
              data.value.forEach(l => {
                console.info(`MetaDataContext: Get listNames from server.`);
                if (l.Title === 'SPMO') {
                  metadataInstance.currentUser.IsSPMO = true;
                }
                metadataInstance.listNames.push(l.Title);
              });
              flag = flag | 4;
            });
          });

          getDeptMappings.then((response: SPHttpClientResponse) => {
            metadataInstance.deptMappings = [];
            response.json().then(dataList => {
              dataList.value.forEach(data => {
                console.info(`MetaDataContext: Get deptMappings from server.`);
                metadataInstance.deptMappings.push({
                  ID: data.ID,
                  Title: data.Title,
                  PPP_Department: data.PPP_Department
                });
              });
              flag = flag | 8;
            });
          });

          getEnhanceSatificationTypes.then((response: SPHttpClientResponse) => {
            metadataInstance.enhanceSatificationTypes = [];
            response.json().then(dataList => {
              dataList.value.forEach(data => {
                console.info(`MetaDataContext: Get enhanceSatificationTypes from server.`);
                metadataInstance.enhanceSatificationTypes.push({
                  ID: data.ID,
                  Title: data.Title,
                  TypeAScore: data.PPP_Score,
                  TypeBScore: data.TypeBScore
                });
              });
              flag = flag | 16;
            });
          });

          getImprovedKPITypes.then((response: SPHttpClientResponse) => {
            metadataInstance.improvedKPITypes = [];
            response.json().then(dataList => {
              dataList.value.forEach(data => {
                console.info(`MetaDataContext: Get improvedKPITypes from server.`);
                metadataInstance.improvedKPITypes.push({
                  ID: data.ID,
                  Title: data.Title,
                  TypeAScore: data.PPP_Score,
                  TypeBScore: data.TypeBScore
                });
              });
              flag = flag | 32;
            });
          });

          getProjectTypes.then((response: SPHttpClientResponse) => {
            metadataInstance.projectTypes = [];
            response.json().then(dataList => {
              dataList.value.forEach(data => {
                console.info(`MetaDataContext: Get projectTypes from server.`);
                metadataInstance.projectTypes.push({
                  ID: data.ID,
                  Title: data.Title,
                  QueueName: data.QueueName,
                  QueueNumber: data.QueueNumber,
                  PrioritizedMatrix: data.PrioritizedMatrix,
                  ProjectCategory: data.ProjectCategory
                });
              });
              flag = flag | 64;
            });
          });

          getRiskMitigatingTypes.then((response: SPHttpClientResponse) => {
            metadataInstance.riskMitigatingTypes = [];
            response.json().then(dataList => {
              dataList.value.forEach(data => {
                console.info(`MetaDataContext: Get riskMitigatingTypes from server.`);
                metadataInstance.riskMitigatingTypes.push({
                  ID: data.ID,
                  Title: data.Title,
                  TypeAScore: data.PPP_Score,
                  TypeBScore: data.TypeBScore
                });
              });
              flag = flag | 128;
            });
          });

          getSiteUsers.then((response: SPHttpClientResponse) => {
            metadataInstance.SiteUsers = [];
            response.json().then(dataList => {
              dataList.value.forEach(data => {
                console.info(`MetaDataContext: Get SiteUsers from server.`);
                metadataInstance.SiteUsers.push({
                  Id: data.Id,
                  Title: data.Title,
                  LoginName: data.LoginName,
                  Email: data.Email
                });
              });
              flag = flag | 256;
            });
          });

          console.info(`Cache DeptMappings building...`);
          const intervalObj = setInterval(() => {
            console.info(`${flag}`);
            if (flag === 511) {
              clearInterval(intervalObj);
              console.info(`Cache DeptMappings build finished.`);
              metadataInstance.deptMappings.forEach((data) => {
                if (data.Title === metadataInstance.currentUser.ADDepartment) {
                  metadataInstance.currentUser.PPPDepartment = data.PPP_Department;
                }
              });
              cache.add(
                cachePrefix + '_' + CACHE_NAME_MetaDataContexts,
                JSON.stringify(metadataInstance),
                Utilities.dateAdd(new Date(), Intervals.Minute, 30));
              resolve(metadataInstance);
            }
          }, 500);
        },
          (err) => {
            reject(err);
          });
      }
    });
  }
}