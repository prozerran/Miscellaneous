import LandingPageTemplate from './LandingPageTemplate';
import * as jQuery from 'jquery';
import { HttpClient, HttpClientResponse, SPHttpClient } from '@microsoft/sp-http';
import { Version, Environment, EnvironmentType } from '@microsoft/sp-core-library';
import {
  BaseClientSideWebPart,
  IPropertyPaneConfiguration,
  PropertyPaneTextField
} from '@microsoft/sp-webpart-base';
import { escape } from '@microsoft/sp-lodash-subset';

import styles from './LandingPageWebPart.module.scss';
import * as strings from 'LandingPageWebPartStrings';

import { IMetaDataService, MetaDataService, IPPMMetaDataContext } from '../../Service/MetaDataService';

export interface IPageLinks {
  Categories: string;
  Title: string;
  PPPHyperLink: string;
}

export interface ILandingPageWebPartProps {
  description: string;
}

export default class LandingPageWebPart extends BaseClientSideWebPart<ILandingPageWebPartProps> {
  private pageLinks: IPageLinks[];
  private metaService: IMetaDataService;

  protected onInit(): Promise<void> {
    this.metaService = new MetaDataService(this.context);
    return super.onInit();
  }

  public render(): void {
    this.domElement.innerHTML = LandingPageTemplate.templateHtml;
    let currentContext = this.context;

    this.metaService.getMetaData().then((meta: IPPMMetaDataContext) => {
      var listName: string = 'Para_PageLinks';
      var listColumns: string[] = ['Categories', 'Title', 'PPPHyperLink'];
      var options = {
        expand: ['Author', 'ProjectOwner', 'ProjectManager', 'ITPmUser'],
        filter: 'PPP_IsActive eq 1',
        orderby: 'PPP_Order'
      };

      let query = currentContext.pageContext.web.absoluteUrl +
        `/_api/Lists/GetByTitle('${listName}')/Items?$Select=${encodeURI(listColumns.join(','))}`;
      if (options && options.orderby) {
        query += `&$orderby=${options.orderby}`;
      }
      if (options && options.filter) {
        query += `&$filter=${options.filter}`;
      }
      currentContext.spHttpClient.get(query,
        SPHttpClient.configurations.v1).then((response: HttpClientResponse) => {
          response.json().then((rows) => {
            var Page_LandingPage = {};
            rows.value.forEach(link => {
              link.PPPHyperLink = link.PPPHyperLink.replace("{currentUserDept}", meta.currentUser.PPPDepartment);
              link.PPPHyperLink = link.PPPHyperLink.replace("{_spPageContextInfo.siteAbsoluteUrl}", this.context.pageContext.site.serverRelativeUrl);
              if(link.Title == 'prc_projectinitation') {
                link.PPPHyperLink = link.PPPHyperLink.replace("{Page_LandingPage.prc_forprojectteam}", Page_LandingPage['prc_forprojectteam']);
              }

              Page_LandingPage[link.Title]=link.PPPHyperLink;
              jQuery(`#${link.Title}`, this.domElement).attr('href', link.PPPHyperLink);
              console.info(`Add landing page link ${link.Title} to ${link.PPPHyperLink}`);
            });

          });
        });
    });
  }

  protected get dataVersion(): Version {
    return Version.parse('1.0');
  }

  protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
    return {
      pages: [
        {
          header: {
            description: strings.PropertyPaneDescription
          },
          groups: [
            {
              groupName: strings.BasicGroupName,
              groupFields: [
                PropertyPaneTextField('description', {
                  label: strings.DescriptionFieldLabel
                })
              ]
            }
          ]
        }
      ]
    };
  }
}
