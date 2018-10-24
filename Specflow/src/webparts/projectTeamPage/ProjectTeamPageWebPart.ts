import { Version, Environment, EnvironmentType } from '@microsoft/sp-core-library';
import {
  BaseClientSideWebPart,
  IPropertyPaneConfiguration,
  PropertyPaneTextField
} from '@microsoft/sp-webpart-base';
import { escape } from '@microsoft/sp-lodash-subset';
import { HttpClient, HttpClientResponse, SPHttpClient } from '@microsoft/sp-http';
import styles from './ProjectTeamPageWebPart.module.scss';
import * as strings from 'ProjectTeamPageWebPartStrings';

import { Grid, GridOptions, ColGroupDef, ColDef, IDatasource, IGetRowsParams, FilterChangedEvent, SortChangedEvent, PostProcessPopupParams, GetContextMenuItemsParams } from 'ag-grid/main';
import 'ag-grid/dist/styles/ag-grid.css';
import 'ag-grid/dist/styles/ag-theme-balham.css';
import { LicenseManager } from "ag-grid-enterprise/main";
LicenseManager.setLicenseKey("Evaluation_License_Valid_Until_11_July_2018__MTUzMTI2MzYwMDAwMA==66e6498f99517f3297ec271d08fa6183");

import f from 'odata-filter-builder';
import * as moment from 'moment';

import { IMetaDataService, MetaDataService, IPPMMetaDataContext } from '../../Service/MetaDataService';

export interface IProjectRequests {
  ID: number;
  Title: string;
  ProjectTypeName: string;
  ProjectName: string;
  CreationDate: Date;
  RaiseDate: Date;
  RequestDept: string;
  Author: string;
  AuthorId: number;
  ProjectOwner: string;
  ProjectOwnerId: number;
  ProjectManager: string;
  ProjectManagerId: number;
  ITPmUser: string;
  ITPmUserId: number;
  ExpectedStartDate: Date;
  ITCostAMT4PHYear0: number;
  ITCostAMT4ConYear0: number;
  OutOfPPPFlag: boolean;
  PrioritizationStatus: string;
}

export interface IProjectTeamPageWebPartProps {
  description: string;
}

export default class ProjectTeamPageWebPart extends BaseClientSideWebPart<IProjectTeamPageWebPartProps> {
  private metaService: IMetaDataService;
  private aGrid: Grid;

  protected onInit(): Promise<void> {
    this.metaService = new MetaDataService(this.context);
    return super.onInit();
  }

  public render(): void {
    let webpartContext = this.context;
    this.domElement.innerHTML = `<div id="myGrid" class="${styles.projectTeamPage} ag-theme-balham"></div>`;
    let eGridDiv: HTMLElement = <HTMLElement>this.domElement.querySelector('#myGrid');

    let currentContext = this.context;
    let currentMetaService = this.metaService;
    let gridOptions: GridOptions = <GridOptions>{
      enableSorting: true,
      enableFilter: true,
      rowSelection: 'multiple',
      rowDeselection: true,
      columnDefs: [
        <ColDef>{ headerName: 'ID', field: 'ID', hide: true },
        <ColDef>{ headerName: 'Project Code', field: 'Title', width: 84 },
        <ColDef>{
          headerName: 'Project Type',
          field: 'ProjectTypeName'
        },
        <ColDef>{ headerName: 'Project Name', field: 'ProjectName', width: 174 },
        <ColDef>{ headerName: 'Creation Date', field: 'CreationDate', type: 'dateColumn', sort: 'asc' },
        <ColDef>{ headerName: 'Project Req Submission Date', field: 'RaiseDate', type: 'dateColumn' },
        <ColDef>{
          headerName: 'Requestor\'s Dept',
          field: 'RequestDept'
        },
        <ColDef>{ headerName: 'Project Requestor', field: 'Author', type: 'userColumn' },
        <ColDef>{ headerName: 'Project Sponsor', field: 'ProjectOwner', type: 'userColumn' },
        <ColDef>{ headerName: 'Proj Mgr', field: 'ProjectManager', type: 'userColumn' },
        <ColDef>{ headerName: 'IT Proj Mgr', field: 'ITPmUser', type: 'userColumn' },
        <ColDef>{ headerName: 'Expected Completion Date', field: 'ExpectedStartDate', type: 'dateColumn' },
        <ColDef>{ headerName: 'IT perm man days', field: 'ITCostAMT4PHYear0', type: 'numberColumn' },
        <ColDef>{ headerName: 'IT contract man days', field: 'ITCostAMT4ConYear0', type: 'numberColumn' },
        <ColDef>{ headerName: 'pre-emptive', field: 'OutOfPPPFlag' },
        <ColDef>{
          headerName: 'Project Request Status',
          field: 'PrioritizationStatus',
          width: 237,
          filter: 'agSetColumnFilter',
          filterParams: {
            cellHeight: 20,
            values: [
              'Pending',
              'Pending for Project Prioritization',
              'Project Accepted',
              'Project Not Accepted'],
            debounceMs: 1000
          }
        }
      ],
      defaultColDef: <ColDef>{
        width: 139,
        editable: false
      },
      defaultColGroupDef: <ColGroupDef>{},
      columnTypes: {
        'dateColumn': {
          width: 84,
          filter: 'agDateColumnFilter',
          cellFormatter: ( (data)=> {
            if(data.value){
              return moment(data.value).format('L');
            } else{
              return '';
            }
          })
        },
        'userColumn': {},
        'numberColumn': {}
      },
      //rowModelType: 'infinite',
      domLayout: 'autoHeight',
      pagination: true,
      paginationPageSize: 20,
      cacheOverflowSize: 2,
      maxConcurrentDatasourceRequests: 2,
      infiniteInitialRowCount: 1,
      maxBlocksInCache: 2,
      toolPanelSuppressSideButtons: true,
      enableRangeSelection: true,
      getMainMenuItems: ((mainMenuItemsParams) => {
        var mainMenuItems = [];
        var itemsToExclude = [
          //'separator', 'pinSubMenu', 'valueAggSubMenu', 'autoSizeThis', 'autoSizeAll', 'rowGroup', 'rowUnGroup',
          //'paste', 'separator', 'export', 
          'toolPanel'
        ];
        mainMenuItemsParams.defaultItems.forEach((item) => {
          if (itemsToExclude.indexOf(item) < 0) {
            mainMenuItems.push(item);
          }
        });
        console.log(mainMenuItems);
        return mainMenuItems;
      }),
      getContextMenuItems: ((contextMenuParams: GetContextMenuItemsParams) => {
        var selectedNodes = contextMenuParams.api.getSelectedNodes();
        console.log(selectedNodes);
        console.log(contextMenuParams.node);
        var countryMenuItems = [];
        countryMenuItems.push({
          name: 'View form'
        });
        countryMenuItems.push({
          name: 'Edit form'
        });
        countryMenuItems.push({
          name: 'Print form'
        });
        countryMenuItems.push({
          name: 'View Summary'
        });
        countryMenuItems.push({
          name: 'Print Summary'
        });
        countryMenuItems.push({
          name: 'View Version history'
        });
        countryMenuItems.push({
          name: 'Delete item'
        });
        countryMenuItems.push('separator');

        var itemsToExclude = [
          //'separator', 'pinSubMenu', 'valueAggSubMenu', 'autoSizeThis', 'autoSizeAll', 'rowGroup', 'rowUnGroup',
          'paste', 'separator', 'export', 'toolPanel'
        ];
        contextMenuParams.defaultItems.forEach((item) => {
          if (itemsToExclude.indexOf(item) < 0) {
            countryMenuItems.push(item);
          }
        });
        console.log(countryMenuItems);
        return countryMenuItems;
      }),
      allowContextMenuWithControlKey: true,
      // datasource: this.projectTeamDataSource,
      getRowNodeId: ((item) => {
        return item.ID.toString();
      })
    };
    this.aGrid = new Grid(eGridDiv, gridOptions);


    currentMetaService.getMetaData().then((meta: IPPMMetaDataContext) => {
      var listName: string = 'Project Request Initiation';
      var listColumns: string[] = ['ID', 'Title', 'ProjectTypeName',
        'ProjectName', 'CreationDate', 'RaiseDate',
        'RequestDept', 'Author/Title', 'ProjectOwner/Title',
        'ProjectManager/Title', 'ITPmUser/Title',
        'ExpectedStartDate', 'ITCostAMT4PHYear0', 'ITCostAMT4ConYear0', 'OutOfPPPFlag',
        'PrioritizationStatus'];
      var expandColumns: string[] = ['Author', 'ProjectOwner', 'ProjectManager', 'ITPmUser'];
      var stepSize: number = 100;
      var nextLink: string = '';
      var filter = '';
      if (!meta.currentUser.IsSPMO) {
        filter = f('or')
          .or(f('or')
            .or(f('or')
              .eq('Author/Id', meta.currentUser.Id)
              .eq('ProjectOwner/Id', meta.currentUser.Id))
            .or(f('or')
              .eq('ProjectManager/Id', meta.currentUser.Id)
              .eq('ITPmUser/Id', meta.currentUser.Id)))
          .or(`substringof('${meta.currentUser.PPPDepartment}',RequestDept)`)
          .toString();
      }
      var query = currentContext.pageContext.web.absoluteUrl +
        `/_api/Lists/GetByTitle('${listName}')/Items?$Select=${encodeURI(listColumns.join(','))}`;
      query += `&$expand=${encodeURI(expandColumns.join(','))}`;
      query += `&$orderby=ID`;
      query += `&$top=${stepSize}`;
      if (filter.length > 0) {
        query += `&$filter=${filter}`;
      }
      function LoadData(queryString: string): Promise<any[]> {
        console.log(queryString);
        return new Promise<any[]>((resolve, reject) => {

          currentContext.spHttpClient.get(
            queryString,
            SPHttpClient.configurations.v1)
            .then((response: HttpClientResponse) => {
              response.json().then((rows) => {
                nextLink = rows['@odata.nextLink'];
                var dataList = [];
                rows.value.forEach(d => {
                  var dataRow = {} as IProjectRequests;
                  listColumns.forEach(c => {
                    if (c === 'Author/Title') {
                      if (d.Author) {
                        dataRow.Author = d.Author.Title;
                      }
                    } else if (c === 'ProjectOwner/Title') {
                      if (d.ProjectOwner) {
                        dataRow.ProjectOwner = d.ProjectOwner.Title;
                      }
                    } else if (c === 'ProjectManager/Title') {
                      if (d.ProjectManager) {
                        dataRow.ProjectManager = d.ProjectManager.Title;
                      }
                    } else if (c === 'ITPmUser/Title') {
                      if (d.ITPmUser) {
                        dataRow.ITPmUser = d.ITPmUser.Title;
                      }
                    } else {
                      dataRow[c] = d[c];
                    }
                  });
                  dataList.push(dataRow);
                });
                console.log(dataList);
                if (dataList.length < stepSize) {
                  console.log('Data load finish.');
                  resolve(dataList);
                } else {
                  LoadData(nextLink).then((nextRows) => {
                    resolve(dataList.concat(nextRows));
                  });
                }
              },
                err => {
                  console.log(err);
                });
            });
        });
      }
      LoadData(query).then((rows) => {
        gridOptions.api.setRowData(rows);
      });
    });
    gridOptions.getRowNodeId = ((item) => {
      return item.ID.toString();
    });
    //gridOptions.api.doLayout();
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
