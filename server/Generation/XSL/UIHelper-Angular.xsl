<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">


'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\list.txt]
  <xsl:for-each select="items/item">
  <xsl:value-of select="@name"/><xsl:text>
  </xsl:text>
  </xsl:for-each>

'''[ENDFILE]


'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\routes.txt]
<xsl:for-each select="items/item[@uiGenerate='true']">
  <xsl:variable name="name_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="name_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="name_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$name_plural"/></xsl:call-template></xsl:variable>
import { <xsl:value-of select="@name"/>ListComponent } from './<xsl:value-of select="$name_plural_lower"/>/<xsl:value-of select="$name_lower"/>-list/<xsl:value-of select="$name_lower"/>-list.component';</xsl:for-each>


  <xsl:for-each select="items/item[@uiGenerate='true']">
  <xsl:variable name="name_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="name_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$name_plural"/></xsl:call-template></xsl:variable>
   {
      path: '<xsl:value-of select="$name_plural_lower"/>',
      component: <xsl:value-of select="@name"/>ListComponent,
      data: { title: '<xsl:value-of select="@friendlyName"/>', breadcrumb: '<xsl:value-of select="@friendlyName"/>' }
   },</xsl:for-each>


MENU ITEMS

  <xsl:for-each select="items/item[@uiGenerate='true']">
  <xsl:sort select="@friendlyName" data-type="text" order="ascending"/>
  <xsl:variable name="name_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="name_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$name_plural"/></xsl:call-template></xsl:variable>
  {
    name: '<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@friendlyName"/></xsl:call-template>',
    type: 'link',
    icon: 'check_box_outline_blank',
    state: '/admin/<xsl:value-of select="$name_plural_lower"/>'
  },</xsl:for-each>

'''[ENDFILE]

'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\_module.ts]
<xsl:for-each select="items/item[@uiGenerate='true']">
  <xsl:variable name="name_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="name_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$name_plural"/></xsl:call-template></xsl:variable>
import { <xsl:value-of select="$name_plural"/>Module } from './<xsl:value-of select="$name_plural_lower"/>/<xsl:value-of select="$name_plural_lower"/>.module';</xsl:for-each>


@NgModule({
  imports: [
  <xsl:for-each select="items/item[@uiGenerate='true']">
  <xsl:variable name="name_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
    <xsl:value-of select="$name_plural"/>Module<xsl:if test="position() != last()">,
    </xsl:if></xsl:for-each>
  ],
  providers: [Title]
})
export class AdminModule {}

'''[ENDFILE]

'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\_en.json]
<xsl:for-each select="items/item[@uiGenerate='true']">
  <xsl:variable name="name_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
"_<xsl:value-of select="$name_lower"/>": {
  "_name": "<xsl:value-of select="@friendlyName"/>",
  <xsl:for-each select="field">"<xsl:value-of select="text()"/>": "<xsl:value-of select="@friendlyName"/>"<xsl:if test="position() != last()">,
  </xsl:if></xsl:for-each><xsl:for-each select="indexfield">,
  "<xsl:value-of select="text()"/>": "<xsl:value-of select="@friendlyName"/>"</xsl:for-each>
},
</xsl:for-each>
<xsl:for-each select="items/enum">
  <xsl:variable name="name_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
"_<xsl:value-of select="$name_lower"/>": {
  <xsl:for-each select="field">"<xsl:value-of select="@value"/>": "<xsl:choose><xsl:when test="string-length(@friendlyName)>0"><xsl:value-of select="@friendlyName"/></xsl:when><xsl:otherwise><xsl:value-of select="text()"/></xsl:otherwise></xsl:choose>"<xsl:if test="position() != last()">,
  </xsl:if></xsl:for-each>
},</xsl:for-each>
'''[ENDFILE]



<xsl:for-each select="items/item[@uiGenerate='true']">
  <xsl:variable name="project"><xsl:value-of select="../@projectName"/></xsl:variable>
  <xsl:variable name="project_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@projectName"/></xsl:call-template></xsl:variable>
  <xsl:variable name="name"><xsl:value-of select="@name"/></xsl:variable>
  <xsl:variable name="name_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="name_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$name_plural"/></xsl:call-template></xsl:variable>
  <xsl:variable name="name_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="name_upper"><xsl:call-template name="ToUpper"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="removePattern2"><xsl:value-of select="@removePattern"/></xsl:variable>
  <xsl:variable name="uiDisplayField"><xsl:choose><xsl:when test="string-length(@uiDisplayField)>0"><xsl:value-of select="@uiDisplayField"/></xsl:when><xsl:when test="count(field[@type='string'])>0"><xsl:value-of select="field[@type='string'][1]/text()"/></xsl:when><xsl:otherwise><xsl:value-of select="field[1]/text()"/></xsl:otherwise></xsl:choose></xsl:variable>



'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-list\<xsl:value-of select="$name_lower"/>-list.component.scss]


'''[ENDFILE]

'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-list\<xsl:value-of select="$name_lower"/>-list.component.html]
&lt;div class="page-layout fullwidth"&gt;
&lt;mat-card &gt;
  &lt;div fxLayout="row" fxLayoutAlign="space-between center"&gt;
    &lt;div fxLayout="row" fxLayoutAlign="start center"&gt;
      &lt;label for="search"&gt;&lt;mat-icon&gt;search&lt;/mat-icon&gt;&lt;/label&gt;
      &lt;mat-form-field class="ml-1"&gt;
        &lt;input matInput placeholder="{{ 'general.search' | translate }}" (keyup)='doSearchDelayed($event)' autocomplete="off"&gt;
      &lt;/mat-form-field&gt;
    &lt;/div&gt;
    &lt;button (click)="showCreate<xsl:value-of select="@name"/>()" mat-raised-button color="primary"&gt;{{ 'general.createNew' | translate }}&lt;/button&gt;
  &lt;/div&gt;
&lt;/mat-card&gt;

&lt;section&gt;
  &lt;mat-card class="p-0 m-1"&gt;
    &lt;ngx-datatable class="material" [rows]="rows" [loadingIndicator]="loadingIndicator" [columnMode]="'force'" [headerHeight]="48"
      [footerHeight]="56" [rowHeight]="'auto'" [scrollbarH]="false" [externalPaging]="true" [count]="tableSettings.paging.total_items"
      [offset]="tableSettings.paging.offset" [limit]="tableSettings.paging.page_size" [externalSorting]="true" [sortType]="'single'"
      [sorts]="tableSettings.sorting" (sort)="onSortClick($event)" (page)='onPagerClick($event)'&gt;
        <xsl:for-each select="field[string-length(@priorityGroupBy)>0]">
        &lt;ngx-datatable-column prop="<xsl:value-of select="text()"/>"&gt; 
          &lt;ng-template let-column="column" ngx-datatable-header-template let-sort="sortFn" let-sortDir="sortDir"&gt;
            &lt;span class="column-header" (click)="sort()"&gt;{{ 'general.priority' | translate }}&lt;/span&gt;
          &lt;/ng-template&gt;
          &lt;ng-template let-value="row" ngx-datatable-cell-template&gt;
            &lt;button mat-button (click)="showEditPriority(value)"&gt;
              {{value.<xsl:value-of select="text()"/>}}
            &lt;/button&gt;
          &lt;/ng-template&gt;
        &lt;/ngx-datatable-column&gt;
        </xsl:for-each>

        <xsl:for-each select="field[not(@uiListHidden='true') and string-length(@foreignKey)=0]">
        <xsl:if test="position()!=1 and string-length(@priorityGroupBy)=0">
        &lt;ngx-datatable-column prop="<xsl:value-of select="text()"/>"&gt;
          &lt;ng-template let-column="column" ngx-datatable-header-template let-sort="sortFn" let-sortDir="sortDir"&gt;
            &lt;span class="column-header" <xsl:if test="@type!='string' or @sortable='true'"> (click)="sort()"</xsl:if>&gt;{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}&lt;/span&gt;
          &lt;/ng-template&gt;
          &lt;ng-template let-value="value" ngx-datatable-cell-template&gt;
            <xsl:choose><xsl:when test="@isEnum='true'">{{ '_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@type"/></xsl:call-template>.'+value |translate}}</xsl:when><xsl:otherwise>{{ value }}</xsl:otherwise></xsl:choose>
          &lt;/ng-template&gt;
        &lt;/ngx-datatable-column&gt;
        </xsl:if>
        </xsl:for-each>

        <xsl:for-each select="indexfield[not(@uiListHidden='true') and @type='string']">
        &lt;ngx-datatable-column prop="<xsl:value-of select="text()"/>"&gt;
          &lt;ng-template let-column="column" ngx-datatable-header-template let-sort="sortFn" let-sortDir="sortDir"&gt;
            &lt;span class="column-header" (click)="sort()"&gt;{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}&lt;/span&gt;
          &lt;/ng-template&gt;
          &lt;ng-template let-value="value" ngx-datatable-cell-template&gt;
            {{ value }}
          &lt;/ng-template&gt;
        &lt;/ngx-datatable-column&gt;
        </xsl:for-each>
        &lt;ngx-datatable-column prop="<xsl:value-of select="field[1]/text()"/>" [cellClass]="'edit-cell'" [sortable]="false"&gt;
          &lt;ng-template let-column="column" ngx-datatable-header-template&gt;
          &lt;/ng-template&gt;
          &lt;ng-template ngx-datatable-cell-template let-value="row"&gt;
          <xsl:choose>
          <xsl:when test="@uiDetail='true'">
            &lt;button mat-icon-button [routerLink]="nav.ADMIN_<xsl:value-of select="$name_upper"/>_DETAIL(value)" &gt;
              &lt;mat-icon class="mat-24" aria-label="View"&gt;east&lt;/mat-icon&gt;
            &lt;/button&gt;
          </xsl:when>
          <xsl:otherwise>
            &lt;button mat-icon-button (click)="showEdit<xsl:value-of select="@name"/>(value)"&gt;
              &lt;mat-icon class="mat-24" aria-label="Edit"&gt;edit&lt;/mat-icon&gt;
            &lt;/button&gt;
          </xsl:otherwise>
          </xsl:choose>
            <xsl:variable name="selfName"><xsl:value-of select="@name"/></xsl:variable>
          &lt;/ng-template&gt;
        &lt;/ngx-datatable-column&gt;
      &lt;/ngx-datatable&gt;
  &lt;/mat-card&gt;
&lt;/section&gt;
&lt;/div&gt;
'''[ENDFILE]
'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_plural_lower"/>.module.ts]
import { NgModule } from '@angular/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { <xsl:value-of select="@name"/>UpdateComponent } from './<xsl:value-of select="$name_lower"/>-update/<xsl:value-of select="$name_lower"/>-update.component';
import { <xsl:value-of select="@name"/>CreateComponent } from './<xsl:value-of select="$name_lower"/>-create/<xsl:value-of select="$name_lower"/>-create.component';
import { <xsl:value-of select="@name"/>ListComponent } from './<xsl:value-of select="$name_lower"/>-list/<xsl:value-of select="$name_lower"/>-list.component';<xsl:if test="@uiDetail='true'">
import { <xsl:value-of select="@name"/>DetailComponent } from './<xsl:value-of select="$name_lower"/>-detail/<xsl:value-of select="$name_lower"/>-detail.component';</xsl:if>
import { MatInputModule } from '@angular/material/input';<xsl:for-each select="../item[@uiParent=$name]"><xsl:variable name="currentChild"><xsl:value-of select="@name"/></xsl:variable>
<xsl:variable name="currentChildPlural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="$currentChild"/></xsl:call-template></xsl:variable>
import { <xsl:value-of select="$currentChildPlural"/>Module } from '../<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$currentChildPlural"/></xsl:call-template>/<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$currentChildPlural"/></xsl:call-template>.module';</xsl:for-each>
@NgModule({
  imports: [
    NgxDatatableModule,
    SharedModule,
    RouterModule,
    MatInputModule<xsl:for-each select="../item[@uiParent=$name]">,
    <xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>Module</xsl:for-each>
  ],
  declarations: [
    <xsl:value-of select="@name"/>ListComponent,
    <xsl:value-of select="@name"/>CreateComponent,
    <xsl:value-of select="@name"/>UpdateComponent<xsl:if test="@uiDetail='true'">,
    <xsl:value-of select="@name"/>DetailComponent</xsl:if>
  ],
  entryComponents: [
    <xsl:value-of select="@name"/>ListComponent,
    <xsl:value-of select="@name"/>CreateComponent,
    <xsl:value-of select="@name"/>UpdateComponent<xsl:if test="@uiDetail='true'">,
    <xsl:value-of select="@name"/>DetailComponent</xsl:if>
  ]<xsl:if test="string-length(@uiParent)>0">,
  exports: [
    <xsl:value-of select="@name"/>ListComponent
  ]</xsl:if>
})
export class <xsl:value-of select="$name_plural"/>Module {}

'''[ENDFILE]

<xsl:choose>
<xsl:when test="string-length(@uiParent)>0">

</xsl:when>
<xsl:otherwise>

</xsl:otherwise>
</xsl:choose>


'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-list\<xsl:value-of select="$name_lower"/>-list.component.ts]
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import {
  ChangeDetectorRef,
  Component,
  Inject,
  Input,
  OnInit
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs/Subject';
import { <xsl:value-of select="$project"/>SDK } from 'app/shared/services/<xsl:value-of select="$project_lower"/>/<xsl:value-of select="$project_lower"/>-sdk';
import { <xsl:value-of select="@name"/>CreateComponent } from '../<xsl:value-of select="$name_lower"/>-create/<xsl:value-of select="$name_lower"/>-create.component';
import { <xsl:value-of select="@name"/>UpdateComponent } from '../<xsl:value-of select="$name_lower"/>-update/<xsl:value-of select="$name_lower"/>-update.component';
import { Observable } from 'rxjs/Observable';
import { NavigationService } from 'app/shared/services/navigation.service';
import { AppAlertService } from 'app/shared/services/app-alert/app-alert.service';

<xsl:if test="count(field[string-length(@priorityGroupBy)>0])>0">
import { PriorityUpdateDialogComponent } from 'app/views/admin/priority-update-dialog/priority-update-dialog.component';</xsl:if>

@Component({
  selector: 'app-<xsl:value-of select="$name_lower"/>-list',
  templateUrl: './<xsl:value-of select="$name_lower"/>-list.component.html',
  styleUrls: ['./<xsl:value-of select="$name_lower"/>-list.component.scss']
})
export class <xsl:value-of select="@name"/>ListComponent implements OnInit {
  <xsl:if test="string-length(@uiParent)>0">@Input() <xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@uiParent"/></xsl:call-template>;</xsl:if>
  emitSearch: any;
  errorMessage: string;
  loadingIndicator = true;
  rows: any;
  delaySearch: Subject&lt;string&gt; = new Subject&lt;string&gt;();
  tableSettings: any;


  constructor(
    private <xsl:value-of select="$project_lower"/>SDK: <xsl:value-of select="$project"/>SDK,
    private alerts: AppAlertService,
    public dialog: MatDialog,
    public nav: NavigationService
  ) {

    this.emitSearch = this.delaySearch
      .asObservable()
      .debounceTime(350)
      .distinctUntilChanged();

    this.emitSearch.subscribe(term => {
      this.tableSettings.keyword = term;
      this.get<xsl:value-of select="$name_plural"/>(true);
    });
  }

  ngOnInit(): void {
    this.tableSettings = {
      keyword: '',
      paging: {
        current_page: 0,
        offset: 0,
        page_size: 10,
        total_items: 0
      },
      sorting: [
        {
          prop: '<xsl:choose><xsl:when test="count(field[string-length(@priorityGroupBy)>0])>0"><xsl:value-of select="field[string-length(@priorityGroupBy)>0][1]/text()"/></xsl:when><xsl:when test="string-length(@uiDefaultSort)>0"><xsl:value-of select="@uiDefaultSort"/><xsl:if test="../@useIndex='true'">.sort</xsl:if></xsl:when><xsl:when test="count(field[@sortable='true'])>0"><xsl:value-of select="field[@sortable='true'][1]/text()"/><xsl:if test="../@useIndex='true'">.sort</xsl:if></xsl:when></xsl:choose>',
          dir: 'asc'
        }
      ]
    };
    this.get<xsl:value-of select="$name_plural"/>();
  }

  get<xsl:value-of select="$name_plural"/>(reset = false): void {
    const paging = this.tableSettings.paging;
    const sorting = this.tableSettings.sorting[0];
    let skip = paging.offset * paging.page_size;

    let sortField = sorting.prop;
    <xsl:if test="count(field[@searchable='true' and ../@useIndex='true']) > 0">
      if(<xsl:for-each select="field[@searchable='true' and ../@useIndex='true']"><xsl:if test="position() > 1"> || </xsl:if>sortField == "<xsl:value-of select="text()"/>"</xsl:for-each>){
        sortField += '.sort';
      }
      
    </xsl:if>

    if (reset) {
      paging.offset = 0;
      skip = 0;
    }
    <xsl:choose>
    <xsl:when test="string-length(@uiParent)>0 and count(field[@storePartitionKey='true'])>0">
    this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@name"/>.findFor<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/>(<xsl:for-each select="field[@tenant='true' and not(@uiParent='true')]">this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="string-length(@extraSecurityRoute)>0">this.<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@uiParent"/></xsl:call-template>.<xsl:value-of select="@extraSecurityRoute"/>, </xsl:if>this.<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@uiParent"/></xsl:call-template>.<xsl:value-of select="field[@uiParent='true']"/><xsl:value-of select="indexfield[@uiParent='true']"/>, skip, paging.page_size, this.tableSettings.keyword, sortField, sorting.dir === 'desc'</xsl:when>
    <xsl:when test="string-length(@uiParent)=0 and count(field[@storePartitionKey='true'])>0">
    this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@name"/>.findFor<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/>(<xsl:for-each select="field[@tenant='true']">this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="text()"/>, </xsl:for-each>skip, paging.page_size, this.tableSettings.keyword, sortField, sorting.dir === 'desc'</xsl:when>
    <xsl:when test="string-length(@uiParent)>0">
    this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@name"/>.get<xsl:value-of select="@name"/>By<xsl:value-of select="field[@uiParent='true'][1]/@foreignKey"/><xsl:value-of select="indexfield[@uiParent='true'][1]/@friendlyName"/>Async(<xsl:for-each select="field[@tenant='true']">this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="text()"/>, </xsl:for-each>this.<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@uiParent"/></xsl:call-template>.<xsl:value-of select="field[@uiParent='true']"/><xsl:value-of select="indexfield[@uiParent='true']"/>,skip,paging.page_size,sortField,sorting.dir === 'desc',this.tableSettings.keyword</xsl:when>
    <xsl:otherwise>
    this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@name"/>.find(<xsl:for-each select="field[@tenant='true']">this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="text()"/>, </xsl:for-each>skip,paging.page_size,this.tableSettings.keyword,sortField,sorting.dir === 'desc'
    </xsl:otherwise></xsl:choose>).subscribe(data => {
      if (data.success) {
        this.rows = data.items;
        this.tableSettings.sorting = [sorting];
        this.tableSettings.paging = data.paging;
        this.tableSettings.paging.offset = data.paging.current_page - 1;
        this.loadingIndicator = false;
      } else {
        this.alerts.error(data);
      }
    });
  }

  doSearchDelayed(event): void {
    const val = event.target.value.toLowerCase();
    this.delaySearch.next(val);
  }

  onSortClick(event): void {
    this.tableSettings.sorting = event.sorts;
    this.get<xsl:value-of select="$name_plural"/>(true);
  }

  showCreate<xsl:value-of select="@name"/>(): void {
    const dialogRef = this.dialog.open(<xsl:value-of select="@name"/>CreateComponent, {
      panelClass: 'flush-dialog',
      autoFocus: false,
      data: {
        <xsl:for-each select="field[@uiParent='true' or @uiRoute='true']"><xsl:value-of select="text()"/>:this.<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@uiParent"/></xsl:call-template>.<xsl:value-of select="text()"/><xsl:if test="position() != last()">,
        </xsl:if></xsl:for-each>}
      }
    );
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.get<xsl:value-of select="$name_plural"/>();
      }
    });
  }

  showEdit<xsl:value-of select="@name"/>(item): void {
    const dialogRef = this.dialog.open(<xsl:value-of select="@name"/>UpdateComponent, {
      panelClass: 'flush-dialog',
      autoFocus: false,
      data: item
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.get<xsl:value-of select="$name_plural"/>();
      }
    });
  }

  <xsl:if test="count(field[string-length(@priorityGroupBy)>0])>0">
  showEditPriority(item): void {
    const dialogRef = this.dialog.open(PriorityUpdateDialogComponent, {
      panelClass: 'flush-dialog',
      autoFocus: false,
      data: {
        item,
        current: item.priority,
        total: this.tableSettings.paging.total_items,
        saveMethod: this.updatePriority.bind(this)
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.get<xsl:value-of select="$name_plural"/>();
      }
    });
  }

  updatePriority(item, priority): Observable&lt;any&gt; {
    return this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@name"/>.update<xsl:value-of select="@name"/><xsl:value-of select="field[string-length(@priorityGroupBy)>0][1]/@friendlyName"/>Async(item.<xsl:value-of select="field[1]/text()"/>, priority);
  }
  </xsl:if>

  onPagerClick(paging): void {
    this.tableSettings.paging.offset = paging.offset;
    this.get<xsl:value-of select="$name_plural"/>(false);
  }
}
'''[ENDFILE]


'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-update\<xsl:value-of select="$name_lower"/>-update.component.html]
&lt;mat-toolbar class="mat-primary m-0"&gt;
  &lt;div fxFlex fxLayout="row" fxLayoutAlign="space-between center"&gt;
      &lt;span class="title dialog-title"&gt;{{ 'general.edit' | translate }} {{'_<xsl:value-of select="$name_lower"/>._name'| translate}}&lt;/span&gt;
      &lt;button mat-icon-button (click)="dialogRef.close()" aria-label="Close dialog"&gt;
          &lt;mat-icon&gt;close&lt;/mat-icon&gt;
      &lt;/button&gt;
  &lt;/div&gt;
&lt;/mat-toolbar&gt;

&lt;div class="p-16 m-0 crud-content"&gt;
  &lt;form name="updateForm" [formGroup]="<xsl:value-of select="$name_lower"/>UpdateForm" class="event-form"&gt;
<xsl:for-each select="field[string-length(@priorityGroupBy)=0 and not(@uiEditHidden='true') and not(@uiParent='true') and not(@uiRoute='true') and not(@tenant='true')]">
  <xsl:choose>
    <xsl:when test="string-length(@foreignKey)>0">
      <xsl:variable name="foreign"><xsl:value-of select="@foreignKey"/></xsl:variable>
      <xsl:variable name="foreign_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template></xsl:variable>
      <xsl:variable name="foreign_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$foreign_plural"/></xsl:call-template></xsl:variable>
    &lt;mat-form-field class="full-width"&gt;
      <xsl:choose>
      <xsl:when test="@uiSearch='true'">
      &lt;input matInput [matAutocomplete]="auto<xsl:value-of select="text()"/>" #<xsl:value-of select="text()"/> formControlName="<xsl:value-of select="text()"/>" placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate}}"&gt;
      &lt;mat-autocomplete #auto<xsl:value-of select="text()"/>="matAutocomplete" [displayWith]="<xsl:value-of select="text()"/>DisplayWith.bind(this)"&gt;
        &lt;mat-option *ngFor="let item of <xsl:value-of select="$foreign_plural_lower"/>" [value]="item.<xsl:choose><xsl:when test="string-length(../../item[@name=$foreign]/field[1]/text())=0">id</xsl:when><xsl:otherwise><xsl:value-of select="../../item[@name=$foreign]/field[1]/text()"/></xsl:otherwise></xsl:choose>" &gt;
        &lt;span&gt;{{ item.<xsl:choose>
        <xsl:when test="string-length(@uiDisplay)>0"><xsl:value-of select="@uiDisplay" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="count(../../item[@name=$foreign][1]/field[@type='string'])>0">
              <xsl:value-of select="../../item[@name=$foreign][1]/field[@type='string'][1]/text()"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="../../item[@name=$foreign][1]/indexfield[@type='string'][1]/text()"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
        </xsl:choose> }}&lt;/span&gt;
        &lt;/mat-option&gt;
      &lt;/mat-autocomplete&gt;
      </xsl:when>
      <xsl:otherwise>
      &lt;mat-select placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}" formControlName="<xsl:value-of select="text()"/>"&gt;
        &lt;mat-option *ngFor="let item of <xsl:value-of select="$foreign_plural_lower"/>" [value]="item.<xsl:choose><xsl:when test="string-length(../../item[@name=$foreign]/field[1]/text())=0">id</xsl:when><xsl:otherwise><xsl:value-of select="../../item[@name=$foreign]/field[1]/text()"/></xsl:otherwise></xsl:choose>" class="admin-lookup-item" &gt;
          &lt;span class="text-item"&gt;{{ item.<xsl:choose>
          <xsl:when test="string-length(@uiDisplay)>0"><xsl:value-of select="@uiDisplay" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="count(../../item[@name=$foreign][1]/field[@type='string'])>0">
                <xsl:value-of select="../../item[@name=$foreign][1]/field[@type='string'][1]/text()"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="../../item[@name=$foreign][1]/indexfield[@type='string'][1]/text()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
          </xsl:choose> }}&lt;/span&gt;

        &lt;/mat-option&gt;
      &lt;/mat-select&gt;
      </xsl:otherwise>
      </xsl:choose>
      &lt;mat-error *ngIf="<xsl:value-of select="$name_lower"/>UpdateFormError.<xsl:value-of select="text()"/>.required"&gt;
        {{ 'general.required' | translate }}
      &lt;/mat-error&gt;
    &lt;/mat-form-field&gt;
    </xsl:when>
    <xsl:when test="@type='bool'">
      &lt;mat-slide-toggle color="primary" formControlName="<xsl:value-of select="text()"/>"&gt;{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate}}&lt;/mat-slide-toggle&gt;
    </xsl:when>
    <xsl:when test="@isEnum='true'">
      <xsl:variable name="enumType"><xsl:value-of select="@type"/></xsl:variable>
      &lt;mat-form-field class="full-width"&gt;
        &lt;mat-select placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}" formControlName="<xsl:value-of select="text()"/>"&gt;
          <xsl:for-each select="/items/enum[@name=$enumType]/field">
          &lt;mat-option [value]="<xsl:value-of select="@value"/>" class="admin-lookup-item" &gt;
            &lt;span class="text-item"&gt;{{'_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$enumType"/></xsl:call-template>.<xsl:value-of select="@value"/>' |translate}}&lt;/span&gt;
          &lt;/mat-option&gt;</xsl:for-each>
        &lt;/mat-select&gt;
        &lt;mat-error *ngIf="<xsl:value-of select="$name_lower"/>UpdateFormError.<xsl:value-of select="text()"/>.required"&gt;
          {{ 'general.required' | translate }}
        &lt;/mat-error&gt;
      &lt;/mat-form-field&gt;
    </xsl:when>
    <xsl:when test="@isUiTextArea='true'">
      &lt;mat-form-field class="full-width mt-1"&gt;
        &lt;textarea matInput placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}" formControlName="<xsl:value-of select="text()"/>" autocomplete="off" &gt;&lt;/textarea&gt;
          &lt;mat-error *ngIf="<xsl:value-of select="$name_lower"/>UpdateFormError.<xsl:value-of select="text()"/>.required"&gt;
            {{ 'general.required' | translate }}
          &lt;/mat-error&gt;
      &lt;/mat-form-field&gt;
    </xsl:when>
    <xsl:otherwise>
      &lt;mat-form-field class="full-width mt-1"&gt;
        &lt;input matInput placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}" formControlName="<xsl:value-of select="text()"/>" autocomplete="off" &gt;
        &lt;mat-error *ngIf="<xsl:value-of select="$name_lower"/>UpdateFormError.<xsl:value-of select="text()"/>.required"&gt;
          {{ 'general.required' | translate }}
        &lt;/mat-error&gt;
      &lt;/mat-form-field&gt;
    </xsl:otherwise>
  </xsl:choose>
</xsl:for-each>
      &lt;mat-divider class="mt-1"&gt;&lt;/mat-divider&gt;
      &lt;div mat-dialog-actions class="mb-0 mt-1" fxLayout="row" &gt;
      &lt;button (click)="onCancelClick()" [disabled]="isLoading" mat-raised-button &gt;
          {{ 'general.cancel' | translate }}
        &lt;/button&gt;
        &lt;button (click)="delete<xsl:value-of select="@name"/>Submit()" [disabled]="isLoading" mat-button color="warn"&gt;
          {{ 'general.delete' | translate }}
        &lt;/button&gt;
        &lt;div fxFlex&gt;&lt;/div&gt;
        &lt;button (click)="update<xsl:value-of select="@name"/>Submit()" [disabled]="<xsl:value-of select="$name_lower"/>UpdateForm.invalid || isLoading" mat-raised-button color="accent"  &gt;
          {{ 'general.save' | translate }}
        &lt;/button&gt;
      &lt;/div&gt;
  &lt;form&gt;
&lt;div&gt;
'''[ENDFILE]
'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-update\<xsl:value-of select="$name_lower"/>-update.component.scss]

'''[ENDFILE]
'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-update\<xsl:value-of select="$name_lower"/>-update.component.ts]
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppConfirmService } from 'app/shared/services/app-confirm/app-confirm.service';
import { AppAlertService } from 'app/shared/services/app-alert/app-alert.service';
import { <xsl:value-of select="$project"/>SDK } from 'app/shared/services/<xsl:value-of select="$project_lower"/>/<xsl:value-of select="$project_lower"/>-sdk';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-<xsl:value-of select="$name_lower"/>-update',
  templateUrl: './<xsl:value-of select="$name_lower"/>-update.component.html',
  styleUrls: ['./<xsl:value-of select="$name_lower"/>-update.component.scss']
})
export class <xsl:value-of select="@name"/>UpdateComponent implements OnInit {
  errorMessage: string;
  <xsl:value-of select="$name_lower"/>: any = {};
  <xsl:value-of select="$name_lower"/>UpdateFormError: any;
  <xsl:value-of select="$name_lower"/>UpdateForm: FormGroup;
  
  isLoading: boolean;
  <xsl:for-each select="field[string-length(@foreignKey)>0]">
    <xsl:variable name="foreign"><xsl:value-of select="@foreignKey"/></xsl:variable>
    <xsl:variable name="foreign_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template></xsl:variable>
    <xsl:variable name="foreign_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$foreign_plural"/></xsl:call-template></xsl:variable>
    <xsl:value-of select="$foreign_plural_lower"/>: Array&lt;any&gt; = [];
    </xsl:for-each>

  constructor(
    private formBuilder: FormBuilder,
    private confirmService: AppConfirmService,
    private alerts: AppAlertService,
    private translate: TranslateService,
    private <xsl:value-of select="$project_lower"/>SDK: <xsl:value-of select="$project"/>SDK,
    public dialogRef: MatDialogRef&lt;<xsl:value-of select="@name"/>UpdateComponent&gt;,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.<xsl:value-of select="$name_lower"/>UpdateFormError = {
      <xsl:for-each select="field"><xsl:value-of select="text()"/>: {}<xsl:if test="position() != last()">,
      </xsl:if></xsl:for-each>
    };
  }

  ngOnInit(): void {
    this.<xsl:value-of select="$name_lower"/>UpdateForm = this.formBuilder.group({
      <xsl:for-each select="field[string-length(@priorityGroupBy)=0 and not(@uiEditHidden='true')]">
      <xsl:value-of select="text()"/>: [<xsl:choose><xsl:when test="position()=1 ">{ value: this.data.<xsl:value-of select="text()"/>, disabled: true }</xsl:when><xsl:when test="@uiRoute='true' or @uiParent='true' or @uiEditReadOnly='true'">{ value: this.data.<xsl:value-of select="text()"/><xsl:if test="@uiEditReadOnly='true'">, disabled: true</xsl:if>}</xsl:when><xsl:otherwise>''</xsl:otherwise></xsl:choose><xsl:if test="not(@isNullable='true') and not(@type='bool')">, [Validators.required]</xsl:if>]<xsl:if test="position() != last()">,
      </xsl:if></xsl:for-each>
    });

    this.<xsl:value-of select="$name_lower"/>UpdateForm.valueChanges.subscribe(() => {
      this.on<xsl:value-of select="@name"/>FormValuesChanged();
    });

    this.get<xsl:value-of select="@name"/>Data();
    <xsl:if test="count(field[@uiSearch='true' or string-length(@foreignKey)>0])>0">this.loadLookups();</xsl:if>
  }

  <xsl:if test="count(field[@uiSearch='true' or string-length(@foreignKey)>0])>0">
  <xsl:for-each select="field[(@uiSearch='true' or string-length(@foreignKey)>0) and not(@uiParent='true') and not(@uiRoute='true')]">
  <xsl:variable name="foreign"><xsl:value-of select="@foreignKey"/></xsl:variable>
    <xsl:variable name="foreign_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template></xsl:variable>
    <xsl:variable name="foreign_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$foreign_plural"/></xsl:call-template></xsl:variable>
    
  <xsl:value-of select="text()"/>DisplayWith(item) {
    if(!this.<xsl:value-of select="$foreign_plural_lower"/>.length &amp;&amp; this.<xsl:value-of select="$name_lower"/> &amp;&amp; this.<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>){
      return this.<xsl:value-of select="$name_lower"/>.<xsl:choose><xsl:when test="string-length(@uiDisplayReference)>0"><xsl:value-of select="@uiDisplayReference"/>.name</xsl:when><xsl:when test="string-length(@uiDisplay)>0"><xsl:value-of select="@uiDisplay" /></xsl:when><xsl:otherwise><xsl:value-of select="text()" /></xsl:otherwise></xsl:choose>;
    }
    let index = this.<xsl:value-of select="$foreign_plural_lower"/>.findIndex(it => it.<xsl:value-of select="text()"/> === item);
    if (index > -1) {
      return this.<xsl:value-of select="$foreign_plural_lower"/>[index].<xsl:choose><xsl:when test="string-length(@uiDisplay)>0"><xsl:value-of select="@uiDisplay" /></xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="count(../../item[@name=$foreign][1]/field[@type='string'])>0">
              <xsl:value-of select="../../item[@name=$foreign][1]/field[@type='string'][1]/text()"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="../../item[@name=$foreign][1]/indexfield[@type='string'][1]/text()"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>;
    }
    return null;
  }
  </xsl:for-each>
  loadLookups(){
    <xsl:for-each select="field[string-length(@foreignKey)>0 and not(@uiParent='true') and not(@uiRoute='true') and not(@tenant='true')]">
    <xsl:variable name="foreign"><xsl:value-of select="@foreignKey"/></xsl:variable>
    <xsl:variable name="foreign_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template></xsl:variable>
    <xsl:variable name="foreign_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$foreign_plural"/></xsl:call-template></xsl:variable>
    
    <xsl:choose>
    <xsl:when test="string-length(@derivedProperty)>0 or @extraRoute='true'">
    </xsl:when>
    <xsl:when test="not(@uiSearch='true')">
    // <xsl:value-of select="@foreignKey"/>
    this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@foreignKey"/>.find<xsl:if test="count(../../item[@name=$foreign][1]/field[@storePartitionKey='true'])>0">For<xsl:value-of select="../../item[@name=$foreign][1]/field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:if>(<xsl:if test="count(../../item[@name=$foreign][1]/field[@storePartitionKey='true'])>0"><xsl:if test="string-length(../../item[@name=$foreign][1]/@extraSecurityRoute)>0">this.data.<xsl:value-of select="../../item[@name=$foreign][1]/@extraSecurityRoute" />,</xsl:if><xsl:choose>
    <xsl:when test="../../item[@name=$foreign][1]/field[@storePartitionKey='true'][1]/text() = ../../item[@name=$foreign][1]/field[@tenant='true'][1]/text()">this.<xsl:value-of select="$project_lower"/>SDK.</xsl:when>
    <xsl:otherwise>this.data.</xsl:otherwise>
    </xsl:choose><xsl:value-of select="../../item[@name=$foreign][1]/field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>0, 2000).subscribe(
        data => {
         this.<xsl:value-of select="$foreign_plural_lower"/> = data.items;
      },
      error => {
         this.alerts.error(error);
      });
    </xsl:when>
    <xsl:otherwise>
    // <xsl:value-of select="@foreignKey"/>
    this.<xsl:value-of select="$name_lower"/>UpdateForm.controls["<xsl:value-of select="text()"/>"].valueChanges.debounceTime(500).subscribe(keyword => {
      if (!!keyword &amp;&amp; keyword.length > 0) {
        this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@foreignKey"/>.find(0, 15, keyword).subscribe(data => {
          this.<xsl:value-of select="$foreign_plural_lower"/> = data.items;
        });
      }
    });
    </xsl:otherwise>
    </xsl:choose>
    </xsl:for-each>
  }
  </xsl:if>

  on<xsl:value-of select="@name"/>FormValuesChanged(): void {
    for (const field in this.<xsl:value-of select="$name_lower"/>UpdateFormError) {
      if (this.<xsl:value-of select="$name_lower"/>UpdateFormError.hasOwnProperty(field)) {
        this.validateFormField(field);
      }
    }
  }

  validateFormField(field): void {
    this.<xsl:value-of select="$name_lower"/>UpdateFormError[field] = {};
    const control = this.<xsl:value-of select="$name_lower"/>UpdateForm.get(field);

    if (control &amp;&amp; control.dirty &amp;&amp; !control.valid) {
      this.<xsl:value-of select="$name_lower"/>UpdateFormError[field] = control.errors;
    }
  }

   get<xsl:value-of select="@name"/>Data(): void {
    this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@name"/>.get<xsl:value-of select="@name"/>Async(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]">this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="count(field[@storePartitionKey='true'])>0">this.data.<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>this.data.<xsl:value-of select="field[1]/text()"/>).subscribe(
      data => {

        if (data.success) {
          this.<xsl:value-of select="$name_lower"/> = data.item;
          this.<xsl:value-of select="$name_lower"/>UpdateForm.patchValue(data.item);
        } else {
          this.alerts.error(data);
        }
      },
      error => {
        this.alerts.error(error);
      }
    );
  }

   update<xsl:value-of select="@name"/>Submit(): void {
      this.isLoading = true;
      this.<xsl:value-of select="$name_lower"/> = { ...this.<xsl:value-of select="$name_lower"/>, ...this.<xsl:value-of select="$name_lower"/>UpdateForm.value };
      this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@name"/>.update<xsl:value-of select="@name"/>Async(this.data.<xsl:value-of select="field[1]/text()"/>,this.<xsl:value-of select="$name_lower"/>)
        .subscribe(
          data => {
            this.isLoading = false;

            if (data.success) {
              this.<xsl:value-of select="$name_lower"/> = data.item;
              this.dialogRef.close(true);
            } else {
              this.alerts.error(data);
            }
          },
          error => {
            this.isLoading = false;
            this.alerts.error(error);
          }
        );
  }

  delete<xsl:value-of select="@name"/>Submit(): void {
    const alertData = {
        title: this.translate.instant('Really Delete?'),
        message: this.translate.instant('general.confirmDelete', {0:this.translate.instant('_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>._name')}),
        accept: this.translate.instant('Yes, Delete'),
        cancel: this.translate.instant('Cancel')
    };
    this.confirmService.confirm(alertData).subscribe(okay =>
    {
      if(okay){
        this.isLoading = true;
        this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@name"/>.delete<xsl:value-of select="@name"/>Async(<xsl:for-each select="field[@tenant='true']">this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="text()"/>, </xsl:for-each>this.data.<xsl:value-of select="field[1]/text()"/>).subscribe(
          data => {
            this.isLoading = false;
            if (data.success) {
              this.dialogRef.close(true);
            } else {
              this.alerts.error(data);
            }
          },
          error => {
            this.isLoading = false;
            this.alerts.error(error);
          }
        );
      }
    });

  }

  onCancelClick(): void {
    this.dialogRef.close();
  }
}

'''[ENDFILE]

'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-create\<xsl:value-of select="$name_lower"/>-create.component.html]
&lt;mat-toolbar class="mat-primary m-0"&gt;
  &lt;div fxFlex fxLayout="row" fxLayoutAlign="space-between center"&gt;
    &lt;span class="title dialog-title"&gt;{{ 'general.createNew' | translate }}&lt;/span&gt;
    &lt;button  mat-icon-button (click)="dialogRef.close()" aria-label="Close dialog"&gt;
      &lt;mat-icon&gt;close&lt;/mat-icon&gt;
    &lt;/button&gt;
  &lt;/div&gt;
&lt;/mat-toolbar&gt;

&lt;form name="createForm" [formGroup]="<xsl:value-of select="$name_lower"/>CreateForm" class="event-form"&gt;
  &lt;div class="p-16 m-0 mt-1" fxFlex="column"&gt;
  
<xsl:for-each select="field[position()!=1 and string-length(@priorityGroupBy)=0  and not(@uiCreateHidden='true') and not(@uiParent='true') and not(@uiRoute='true') and not(@tenant='true')]">
<xsl:choose>
  <xsl:when test="string-length(@derivedProperty)>0">
    &lt;!-- Could Edit Derived --&gt;
  </xsl:when>
  <xsl:when test="string-length(@foreignKey)>0">
  <xsl:variable name="foreign"><xsl:value-of select="@foreignKey"/></xsl:variable>
  <xsl:variable name="foreign_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template></xsl:variable>
  <xsl:variable name="foreign_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$foreign_plural"/></xsl:call-template></xsl:variable>
  &lt;mat-form-field class="full-width"&gt;
  <xsl:choose>
    <xsl:when test="not(@uiSearch='true')">
      &lt;mat-select formControlName="<xsl:value-of select="text()"/>" placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}"&gt;
        &lt;mat-option *ngFor="let item of <xsl:value-of select="$foreign_plural_lower"/>" [value]="item.<xsl:choose><xsl:when test="string-length(../../item[@name=$foreign]/field[1]/text())=0">id</xsl:when><xsl:otherwise><xsl:value-of select="../../item[@name=$foreign]/field[1]/text()"/></xsl:otherwise></xsl:choose>" class="admin-lookup-item" &gt;
          &lt;span class="text-item"&gt;{{ item.<xsl:choose>
          <xsl:when test="string-length(@uiDisplay)>0"><xsl:value-of select="@uiDisplay" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="count(../../item[@name=$foreign][1]/field[@type='string'])>0">
                <xsl:value-of select="../../item[@name=$foreign][1]/field[@type='string'][1]/text()"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="../../item[@name=$foreign][1]/indexfield[@type='string'][1]/text()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
          </xsl:choose>}}&lt;/span&gt;
        &lt;/mat-option&gt;
      &lt;/mat-select&gt;
    </xsl:when>
    <xsl:otherwise>
      &lt;input matInput [matAutocomplete]="auto<xsl:value-of select="text()"/>" #<xsl:value-of select="text()"/> formControlName="<xsl:value-of select="text()"/>" placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}"&gt;
      &lt;mat-autocomplete #auto<xsl:value-of select="text()"/>="matAutocomplete" [displayWith]="<xsl:value-of select="text()"/>DisplayWith.bind(this)"&gt;
        &lt;mat-option *ngFor="let item of <xsl:value-of select="$foreign_plural_lower"/>" [value]="item.<xsl:choose>
        <xsl:when test="string-length(../../item[@name=$foreign]/field[1]/text())=0">id</xsl:when><xsl:otherwise><xsl:value-of select="../../item[@name=$foreign]/field[1]/text()"/></xsl:otherwise></xsl:choose>" &gt;
        &lt;span&gt;{{ item.<xsl:choose>
        <xsl:when test="string-length(@uiDisplay)>0">
        <xsl:value-of select="@uiDisplay" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="count(../../item[@name=$foreign][1]/field[@type='string'])>0">
              <xsl:value-of select="../../item[@name=$foreign][1]/field[@type='string'][1]/text()"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="../../item[@name=$foreign][1]/indexfield[@type='string'][1]/text()"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
        </xsl:choose> }}&lt;/span&gt;
        &lt;/mat-option&gt;
      &lt;/mat-autocomplete&gt;
    </xsl:otherwise>
  </xsl:choose>
    &lt;mat-error *ngIf="<xsl:value-of select="$name_lower"/>CreateFormError.<xsl:value-of select="text()"/>.required"&gt;
      {{ 'general.required' | translate }}
    &lt;/mat-error&gt;
  &lt;/mat-form-field&gt;
  </xsl:when>
  <xsl:when test="@type='bool'">
    &lt;mat-slide-toggle  class="mt-1 mb-1" color="accent" formControlName="<xsl:value-of select="text()"/>">{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}&lt;/mat-slide-toggle&gt;
  </xsl:when>
  <xsl:when test="@isEnum='true'">
  <xsl:variable name="enumType"><xsl:value-of select="@type"/></xsl:variable>
    &lt;mat-form-field class="full-width"&gt;
      &lt;mat-select placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}" formControlName="<xsl:value-of select="text()"/>"&gt;
        <xsl:for-each select="/items/enum[@name=$enumType]/field">
        &lt;mat-option [value]="<xsl:value-of select="@value"/>" class="admin-lookup-item" &gt;
          &lt;span class="text-item"&gt;{{ '_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$enumType"/></xsl:call-template>.<xsl:value-of select="@value"/>' | translate }}&lt;/span&gt;
        &lt;/mat-option&gt;</xsl:for-each>
      &lt;/mat-select&gt;
      &lt;mat-error *ngIf="<xsl:value-of select="$name_lower"/>CreateFormError.<xsl:value-of select="text()"/>.required"&gt;
        {{ 'general.required' | translate }}
      &lt;/mat-error&gt;
    &lt;/mat-form-field&gt;
  </xsl:when>
  <xsl:when test="@isUiTextArea='true'">
    &lt;mat-form-field class="full-width"&gt;
      &lt;textarea matInput placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}" formControlName="<xsl:value-of select="text()"/>" autocomplete="off" &gt;&lt;/textarea&gt;
        &lt;mat-error *ngIf="<xsl:value-of select="$name_lower"/>CreateFormError.<xsl:value-of select="text()"/>.required"&gt;
          {{ 'general.required' | translate }}
        &lt;/mat-error&gt;
    &lt;/mat-form-field&gt;
  </xsl:when>
  <xsl:otherwise>
    &lt;mat-form-field class="full-width"&gt;
      &lt;input matInput placeholder="{{ '_<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>' | translate }}" formControlName="<xsl:value-of select="text()"/>" autocomplete="off" &gt;
        &lt;mat-error *ngIf="<xsl:value-of select="$name_lower"/>CreateFormError.<xsl:value-of select="text()"/>.required"&gt;
          {{ 'general.required' | translate }}
        &lt;/mat-error&gt;
    &lt;/mat-form-field&gt;
  </xsl:otherwise>
</xsl:choose>
</xsl:for-each>
    &lt;mat-divider class="mt-1"&gt;&lt;/mat-divider&gt;
    &lt;div mat-dialog-actions class="actions mb-0 mt-1" fxLayout="row" fxLayoutAlign="end center"&gt;
      &lt;button (click)="create<xsl:value-of select="$name"/>Submit()" mat-raised-button color="primary" [disabled]="<xsl:value-of select="$name_lower"/>CreateForm.invalid || isLoading"&gt;
        {{ 'general.save' | translate }}
      &lt;/button&gt;
    &lt;/div&gt;
  &lt;/div&gt;
&lt;/form&gt;

'''[ENDFILE]
'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-create\<xsl:value-of select="$name_lower"/>-create.component.scss]

'''[ENDFILE]
'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-create\<xsl:value-of select="$name_lower"/>-create.component.ts]
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { <xsl:value-of select="$project"/>SDK } from 'app/shared/services/<xsl:value-of select="$project_lower"/>/<xsl:value-of select="$project_lower"/>-sdk';
import { AppAlertService } from 'app/shared/services/app-alert/app-alert.service';

@Component({
  selector: 'app-<xsl:value-of select="$name_lower"/>-create',
  templateUrl: './<xsl:value-of select="$name_lower"/>-create.component.html',
  styleUrls: ['./<xsl:value-of select="$name_lower"/>-create.component.scss']
})
export class <xsl:value-of select="@name"/>CreateComponent implements OnInit {
  errorMessage: string;
  <xsl:value-of select="$name_lower"/>: any = {};
  <xsl:value-of select="$name_lower"/>CreateFormError: any;
  <xsl:value-of select="$name_lower"/>CreateForm: FormGroup;

  isLoading: boolean;
  <xsl:for-each select="field[string-length(@foreignKey)>0]">
    <xsl:variable name="foreign"><xsl:value-of select="@foreignKey"/></xsl:variable>
    <xsl:variable name="foreign_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template></xsl:variable>
    <xsl:variable name="foreign_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$foreign_plural"/></xsl:call-template></xsl:variable>
    <xsl:value-of select="$foreign_plural_lower"/>: Array&lt;any&gt; = [];
    </xsl:for-each>

  constructor(
    public dialogRef: MatDialogRef&lt;<xsl:value-of select="@name"/>CreateComponent&gt;,
    private formBuilder: FormBuilder,
    private alerts: AppAlertService,
    private <xsl:value-of select="$project_lower"/>SDK: <xsl:value-of select="$project"/>SDK,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.<xsl:value-of select="$name_lower"/> = data;
    this.<xsl:value-of select="$name_lower"/>CreateFormError = {
      <xsl:for-each select="field"><xsl:value-of select="text()"/>: {}<xsl:if test="position() != last()">,
      </xsl:if></xsl:for-each>
    };
  }

  ngOnInit(): void {
    this.<xsl:value-of select="$name_lower"/>CreateForm = this.formBuilder.group({
      <xsl:for-each select="field[string-length(@priorityGroupBy)=0 and not(@uiCreateHidden='true') and not(@tenant='true')]">
      <xsl:value-of select="text()"/>: [<xsl:choose><xsl:when test="position()=1">{ value: '', disabled: true }</xsl:when><xsl:when test="@uiRoute='true' or @uiParent='true' or @uiEditReadOnly='true'">this.data.<xsl:value-of select="text()"/></xsl:when><xsl:otherwise>''</xsl:otherwise></xsl:choose><xsl:if test="not(@isNullable='true') and not(position()=1) and not(@type='bool')">, [Validators.required]</xsl:if>]<xsl:if test="position() != last()">,
      </xsl:if></xsl:for-each>
      <xsl:for-each select="field[@tenant='true']">,
      <xsl:value-of select="text()"/>: [this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="text()"/>]</xsl:for-each>
    });

    this.<xsl:value-of select="$name_lower"/>CreateForm.valueChanges.subscribe(() => {
      this.on<xsl:value-of select="@name"/>FormValuesChanged();
    });
    <xsl:if test="count(field[(@uiSearch='true' or string-length(@foreignKey)>0) and not(@uiParent='true') and not(@uiRoute='true')])>0">this.loadLookups();</xsl:if>
  }

  <xsl:if test="count(field[(@uiSearch='true' or string-length(@foreignKey)>0) and not(@uiParent='true') and not(@uiRoute='true')])>0">
  <xsl:for-each select="field[(@uiSearch='true' or string-length(@foreignKey)>0) and not(@uiParent='true') and not(@uiRoute='true')]">
  <xsl:variable name="foreign"><xsl:value-of select="@foreignKey"/></xsl:variable>
    <xsl:variable name="foreign_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template></xsl:variable>
    <xsl:variable name="foreign_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$foreign_plural"/></xsl:call-template></xsl:variable>
    
  <xsl:value-of select="text()"/>DisplayWith(item) {
    if(!this.<xsl:value-of select="$foreign_plural_lower"/>.length &amp;&amp; this.<xsl:value-of select="$name_lower"/> &amp;&amp; this.<xsl:value-of select="$name_lower"/>.<xsl:value-of select="text()"/>){
      return this.<xsl:value-of select="$name_lower"/>.<xsl:choose><xsl:when test="string-length(@uiDisplayReference)>0"><xsl:value-of select="@uiDisplayReference"/>.name</xsl:when><xsl:when test="string-length(@uiDisplay)>0"><xsl:value-of select="@uiDisplay" /></xsl:when><xsl:otherwise><xsl:value-of select="text()" /></xsl:otherwise></xsl:choose>;
    }
    let index = this.<xsl:value-of select="$foreign_plural_lower"/>.findIndex(it => it.<xsl:value-of select="text()"/> === item);
    if (index > -1) {
      return this.<xsl:value-of select="$foreign_plural_lower"/>[index].<xsl:choose><xsl:when test="string-length(@uiDisplay)>0"><xsl:value-of select="@uiDisplay" /></xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="count(../../item[@name=$foreign][1]/field[@type='string'])>0">
              <xsl:value-of select="../../item[@name=$foreign][1]/field[@type='string'][1]/text()"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="../../item[@name=$foreign][1]/indexfield[@type='string'][1]/text()"/>
            </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
      </xsl:choose>;
    }
    return null;
  }
  </xsl:for-each>
  loadLookups(){
    <xsl:for-each select="field[string-length(@foreignKey)>0 and not(@uiParent='true') and not(@uiRoute='true') and not(@tenant='true')  ]">
    <xsl:variable name="foreign"><xsl:value-of select="@foreignKey"/></xsl:variable>
    <xsl:variable name="foreign_plural"><xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template></xsl:variable>
    <xsl:variable name="foreign_plural_lower"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="$foreign_plural"/></xsl:call-template></xsl:variable>
    <xsl:choose>
    <xsl:when test="string-length(@derivedProperty)>0 or @extraRoute='true'">
    </xsl:when>
    <xsl:when test="not(@uiSearch='true')">
    // <xsl:value-of select="@foreignKey"/>
    this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@foreignKey"/>.find<xsl:if test="count(../../item[@name=$foreign][1]/field[@storePartitionKey='true'])>0">For<xsl:value-of select="../../item[@name=$foreign][1]/field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:if>(<xsl:if test="count(../../item[@name=$foreign][1]/field[@storePartitionKey='true'])>0"><xsl:if test="string-length(../../item[@name=$foreign][1]/@extraSecurityRoute)>0">this.data.<xsl:value-of select="../../item[@name=$foreign][1]/@extraSecurityRoute" />,</xsl:if><xsl:choose>
    <xsl:when test="../../item[@name=$foreign][1]/field[@storePartitionKey='true'][1]/text() = ../../item[@name=$foreign][1]/field[@tenant='true'][1]/text()">this.<xsl:value-of select="$project_lower"/>SDK.</xsl:when>
    <xsl:otherwise>this.data.</xsl:otherwise>
    </xsl:choose><xsl:value-of select="../../item[@name=$foreign][1]/field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>0, 2000).subscribe(
        data => {
         this.<xsl:value-of select="$foreign_plural_lower"/> = data.items;
      },
      error => {
         this.alerts.error(error);
      });
    </xsl:when>
    <xsl:otherwise>
    // <xsl:value-of select="@foreignKey"/>
    this.<xsl:value-of select="$name_lower"/>CreateForm.controls["<xsl:value-of select="text()"/>"].valueChanges.debounceTime(500).subscribe(keyword => {
      if (!!keyword &amp;&amp; keyword.length > 0) {
        this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@foreignKey"/>.find(0, 15, keyword).subscribe(data => {
          this.<xsl:value-of select="$foreign_plural_lower"/> = data.items;
        });
      }
    });
    </xsl:otherwise>
    </xsl:choose>
    </xsl:for-each>
  }
  </xsl:if>
  on<xsl:value-of select="@name"/>FormValuesChanged(): void {
    for (const field in this.<xsl:value-of select="$name_lower"/>CreateFormError) {
      if (this.<xsl:value-of select="$name_lower"/>CreateFormError.hasOwnProperty(field)) {
        this.validateFormField(field);
      }
    }
  }

  validateFormField(field): void {
    this.<xsl:value-of select="$name_lower"/>CreateFormError[field] = {};
    const control = this.<xsl:value-of select="$name_lower"/>CreateForm.get(field);

    if (control &amp;&amp; control.dirty &amp;&amp; !control.valid) {
      this.<xsl:value-of select="$name_lower"/>CreateFormError[field] = control.errors;
    }
  }

  create<xsl:value-of select="@name"/>Submit(): void {
    this.isLoading = true;
    this.<xsl:value-of select="$name_lower"/> = { ...this.<xsl:value-of select="$name_lower"/>, ...this.<xsl:value-of select="$name_lower"/>CreateForm.value };
    this.<xsl:value-of select="$project_lower"/>SDK.<xsl:value-of select="@name"/>.create<xsl:value-of select="@name"/>Async(this.<xsl:value-of select="$name_lower"/>).subscribe(
      data => {
        this.isLoading = false;
        if (data.success) {
          this.<xsl:value-of select="$name_lower"/> = data.item;

          this.dialogRef.close(true);
        } else {
          this.alerts.error(data);
        }
      },
      error => {
        this.isLoading = false;
        this.alerts.error(error);
      }
    );
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }
}

'''[ENDFILE]

<xsl:if test="@uiDetail='true'">
'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-detail\<xsl:value-of select="$name_lower"/>-detail.component.html]
&lt;ng-container *ngIf="item"&gt;
  &lt;mat-card class="p-0 col-sm-12 card-detail"&gt;
    &lt;mat-card-title class="mat-bg-primary"&gt;
      &lt;div class="card-title-text" fxLayout="row" &gt;
        &lt;span&gt;{{ '_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>._name' | translate }}&lt;/span&gt;
        &lt;div fxFlex&gt;&lt;/div&gt;
        &lt;button (click)="onEdit<xsl:value-of select="@name"/>()" mat-raised-button&gt;{{ 'general.edit' | translate }} <xsl:value-of select="@name" />&lt;/button&gt;
      &lt;/div&gt;
    &lt;/mat-card-title&gt;
    &lt;mat-card-content&gt;
      &lt;div fxLayout="row" fxLayoutAlign="end center" class="actions"&gt;
      &lt;/div&gt;
    &lt;mat-list&gt;
    <xsl:for-each select="field[@uiDetail='true']">
      <xsl:choose>
      <xsl:when test="@derivedProperty='avatar'">
      &lt;!-- Avatar Here--&gt;
      </xsl:when>
      <xsl:otherwise>
      &lt;mat-list-item&gt;
        &lt;mat-icon class="mr-1 text-grey mat-18"&gt;<xsl:choose><xsl:when test="string-length(@uiDetailIcon)>0"><xsl:value-of select="@uiDetailIcon"/></xsl:when><xsl:otherwise>double_arrow</xsl:otherwise></xsl:choose>&lt;/mat-icon&gt; &lt;strong&gt;{{'_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.<xsl:value-of select="text()"/>' | translate}}&lt;/strong&gt;: <xsl:choose><xsl:when test="@isEnum='true'">{{ '_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@type"/></xsl:call-template>.'+item.<xsl:value-of select="text()"/> |translate}}</xsl:when><xsl:otherwise>{{item.<xsl:value-of select="text()"/>}}</xsl:otherwise></xsl:choose>
      &lt;/mat-list-item&gt;
      </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
    <xsl:for-each select="indexfield[@uiDetail='true']">
      <xsl:choose>
      <xsl:when test="@derivedProperty='avatar'">
      &lt;!-- Avatar Here--&gt;
      </xsl:when>
      <xsl:otherwise>
      &lt;mat-list-item&gt;
        &lt;mat-icon class="mr-1 text-grey mat-18"&gt;<xsl:choose><xsl:when test="string-length(@uiDetailIcon)>0"><xsl:value-of select="@uiDetailIcon"/></xsl:when><xsl:otherwise>double_arrow</xsl:otherwise></xsl:choose>&lt;/mat-icon&gt; &lt;strong&gt;{{'_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.<xsl:value-of select="text()"/>' | translate}}&lt;/strong&gt;: <xsl:choose><xsl:when test="@isEnum='true'">{{ '_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@type"/></xsl:call-template>.'+item.<xsl:value-of select="text()"/> |translate}}</xsl:when><xsl:otherwise>{{item.<xsl:value-of select="text()"/>}}</xsl:otherwise></xsl:choose>
      &lt;/mat-list-item&gt;
      </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
    &lt;/mat-list&gt;
    &lt;/mat-card-content&gt;
  &lt;/mat-card&gt;
<xsl:for-each select="../item[@uiParent=$name]">
  <xsl:variable name="group" select="position()"/>
  <xsl:if test="count(../item[@uiParent=$name and @uiParentRow=$group]/field[@uiParent='true'])>0">
   &lt;div fxLayout="row" &gt;&lt;!-- uiParentRow:<xsl:value-of select="$group"/> --&gt;
    <xsl:for-each select="../item[@uiParent=$name and @uiParentRow=$group]/field[@uiParent='true']">
      &lt;mat-card class="p-0 card-detail" fxFlex &gt;
        &lt;mat-card-title class="mat-bg-primary"&gt;
          &lt;div class="card-title-text"&gt;{{'<xsl:value-of select="../@friendlyName"/>' |translate}}&lt;/div&gt;
          &lt;mat-divider&gt;&lt;/mat-divider&gt;
        &lt;/mat-card-title&gt;
        &lt;mat-card-content&gt;
          &lt;app-<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>-list [<xsl:value-of select="$name_lower"/>]="item"&gt;&lt;/app-<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>-list&gt;
        &lt;/mat-card-content&gt;
      &lt;/mat-card&gt;
    </xsl:for-each>
    <xsl:for-each select="../item[@uiParent=$name and @uiParentRow=$group]/indexfield[@uiParent='true']">
      &lt;mat-card class="p-0 card-detail" fxFlex &gt;
        &lt;mat-card-title class="mat-bg-primary"&gt;
          &lt;div class="card-title-text"&gt;{{'<xsl:value-of select="../@friendlyName"/>' |translate}}&lt;/div&gt;
          &lt;mat-divider&gt;&lt;/mat-divider&gt;
        &lt;/mat-card-title&gt;
        &lt;mat-card-content&gt;
          &lt;app-<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>-list [<xsl:value-of select="$name_lower"/>]="item"&gt;&lt;/app-<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>-list&gt;
        &lt;/mat-card-content&gt;
      &lt;/mat-card&gt;
    </xsl:for-each>
   &lt;/div&gt;
  </xsl:if>
</xsl:for-each>
<xsl:for-each select="../item[string-length(@uiParentRow)=0]/field[@foreignKey=$name and @uiParent='true']">
  &lt;mat-card class="p-0 card-detail" &gt;
    &lt;mat-card-title class="mat-bg-primary"&gt;
      &lt;div class="card-title-text"&gt;{{'<xsl:value-of select="../@friendlyName"/>' |translate}}&lt;/div&gt;
      &lt;mat-divider&gt;&lt;/mat-divider&gt;
    &lt;/mat-card-title&gt;
    &lt;mat-card-content&gt;
      &lt;app-<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>-list [<xsl:value-of select="$name_lower"/>]="item"&gt;&lt;/app-<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>-list&gt;
    &lt;/mat-card-content&gt;
  &lt;/mat-card&gt;
</xsl:for-each>
&lt;/ng-container&gt;
'''[ENDFILE]

'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-detail\<xsl:value-of select="$name_lower"/>-detail.component.ts]import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { <xsl:value-of select="$project"/>SDK } from 'app/shared/services/<xsl:value-of select="$project_lower"/>/<xsl:value-of select="$project_lower"/>-sdk';
import { AppAlertService } from 'app/shared/services/app-alert/app-alert.service';
import { MatDialog } from '@angular/material/dialog';
import { <xsl:value-of select="@name"/>UpdateComponent } from '../<xsl:value-of select="$name_lower"/>-update/<xsl:value-of select="$name_lower"/>-update.component';

@Component({
   selector: 'app-<xsl:value-of select="$name_lower"/>-detail',
   templateUrl: './<xsl:value-of select="$name_lower"/>-detail.component.html',
   styleUrls: ['./<xsl:value-of select="$name_lower"/>-detail.component.scss']
})
export class <xsl:value-of select="@name"/>DetailComponent implements OnInit {
   private <xsl:value-of select="field[1]/text()"/>: string;
   <xsl:if test="count(field[@storePartitionKey='true'])>0">private <xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>:string;</xsl:if>
   public item: any;

   constructor(
      private sdk: <xsl:value-of select="$project"/>SDK,
      private alerts: AppAlertService,
      public dialog: MatDialog,
      private activatedRoute: ActivatedRoute) { }

   ngOnInit() {
      this.activatedRoute.params.subscribe(params => {
         this.<xsl:value-of select="field[1]/text()"/> = params['<xsl:value-of select="field[1]/text()"/>'];
         <xsl:if test="count(field[@storePartitionKey='true'])>0 and field[@storePartitionKey='true'][1]/text()=field[@tenant='true'][1]/text()">this.<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/> = this.sdk.<xsl:value-of select="field[@tenant='true'][1]/text()"/>;</xsl:if>
         <xsl:if test="count(field[@storePartitionKey='true'])>0 and field[@storePartitionKey='true'][1]/text()!=field[@tenant='true'][1]/text()">this.<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/> = params['<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>'];</xsl:if>

         this.loadItem();
      });
   }

   loadItem() {

      this.sdk.<xsl:value-of select="@name"/>.get<xsl:value-of select="@name"/>Async(<xsl:if test="count(field[@storePartitionKey='true'])>0">this.<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>this.<xsl:value-of select="field[1]/text()"/>).subscribe(
         data => {
            if (data.success) {
               this.item = data.item;
            } else {
               this.alerts.error(data);
            }
         },
         error => {
            this.alerts.error(error);
         });
   }

   onEdit<xsl:value-of select="@name"/>(): void {
      const dialogRef = this.dialog.open(<xsl:value-of select="@name"/>UpdateComponent, {
        panelClass: 'flush-dialog',
        autoFocus: false,
        data: this.item
      });

      dialogRef.afterClosed().subscribe(result => {
         if (result) {
            this.loadItem();
         }
      });
   }


}

'''[ENDFILE]
'''[STARTFILE:..\Generation\Output\UIHelpers\Angular\<xsl:value-of select="@uiPath"/><xsl:value-of select="$name_plural_lower"/>\<xsl:value-of select="$name_lower"/>-detail\<xsl:value-of select="$name_lower"/>-detail.component.scss]


'''[ENDFILE]
</xsl:if>
</xsl:for-each>

</xsl:template>

	<xsl:template match="@space"> </xsl:template>
  <xsl:template name="ToLower">
          <xsl:param name="inputString"/>
          <xsl:variable name="smallCase" select="'abcdefghijklmnopqrstuvwxyz'"/>
          <xsl:variable name="upperCase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
          <xsl:value-of select="translate($inputString,$upperCase,$smallCase)"/>
  </xsl:template>
 <xsl:template name="ToUpper">
          <xsl:param name="inputString"/>
          <xsl:variable name="smallCase" select="'abcdefghijklmnopqrstuvwxyz'"/>
          <xsl:variable name="upperCase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
          <xsl:value-of select="translate($inputString,$smallCase,$upperCase)"/>
  </xsl:template>
  <xsl:template name="Pluralize">
          <xsl:param name="inputString"/>
          <xsl:choose>
            <xsl:when test="substring($inputString, string-length($inputString)) = 'x'"><xsl:value-of select="$inputString"/>es</xsl:when>
            <xsl:when test="substring($inputString, string-length($inputString)-1) = 'ch'"><xsl:value-of select="$inputString"/>es</xsl:when>
            <xsl:when test="substring($inputString, string-length($inputString)) = 'y'"><xsl:value-of select="concat(substring($inputString, 1, string-length($inputString)-1),'ies')"/></xsl:when>
            <xsl:otherwise><xsl:value-of select="$inputString"/>s</xsl:otherwise></xsl:choose>
  </xsl:template>
</xsl:stylesheet>