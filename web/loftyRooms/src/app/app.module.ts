import { NgModule,CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { LoginComponent } from './pages/account/login/login.component';
import { ClaimComponent } from './pages/account/claim/claim.component';
import { RoleComponent } from './pages/account/role/role.component';
import { UserComponent } from './pages/account/user/user.component';
import { UserRoleComponent } from './pages/account/user-role/user-role.component';
import { RoleClaimsComponent } from './pages/account/role-claims/role-claims.component';
import { LayoutComponent } from './pages/dashboard/layout/layout.component';
import { IndexComponent } from './pages/dashboard/index/index.component';
import { RegisterComponent } from './pages/account/register/register.component';
import { HttpClientModule } from '@angular/common/http';
import { DataTablesModule } from 'angular-datatables';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { DxTemplateModule,DxButtonModule, DxDataGridModule,DxSelectBoxModule } from 'devextreme-angular';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { NoCopyDirective } from './no-copy.directive';
import { AddPartnerComponent } from './pages/partner/add-partner/add-partner.component';
import { PartnerListComponent } from './pages/partner/partner-list/partner-list.component';
import { FacilityComponent } from './pages/setting/facility/facility.component';
import { RoomTypesComponent } from './pages/setting/room-types/room-types.component';
import { PersonComponent } from './pages/setting/person/person.component';
import { AddPackageComponent } from './pages/setting/package/add-package/add-package.component';
import { PackageListComponent } from './pages/setting/package/package-list/package-list.component';
import { AddAdsComponent } from './pages/ads/add-ads/add-ads.component';
import { AdsListComponent } from './pages/ads/ads-list/ads-list.component';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';
import { AddFacilityComponent } from './pages/setting/facility/add-facility/add-facility.component';
import { HelpandsupportComponent } from './pages/setting/helpandsupport/helpandsupport.component';
import { CustomerNotificationsComponent } from './pages/setting/customer-notifications/customer-notifications.component';
import { PartnerNotificationsComponent } from './pages/setting/partner-notifications/partner-notifications.component';
@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  declarations: [
    AppComponent,
    LoginComponent,
    ClaimComponent,
    RoleComponent,
    UserComponent,
    UserRoleComponent,
    RoleClaimsComponent,
    LayoutComponent,
    IndexComponent,
    RegisterComponent,
    NoCopyDirective,
    AddPartnerComponent,
    PartnerListComponent,
    FacilityComponent,
    RoomTypesComponent,
    PersonComponent,
    AddPackageComponent,
    PackageListComponent,
    AddAdsComponent,
    AdsListComponent,
    AddFacilityComponent,
    HelpandsupportComponent,
    CustomerNotificationsComponent,
    PartnerNotificationsComponent,
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    DataTablesModule,
    MatCheckboxModule,
    MatSnackBarModule,
    NgxMatSelectSearchModule,
    NgbModule,
    DxTemplateModule,
    DxDataGridModule,
    DxButtonModule,
    DxSelectBoxModule,
    NgbModalModule,
    MatSelectModule,
    MatFormFieldModule
  ],
  providers: [
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
