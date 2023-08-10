import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/account/login/login.component';
import { RoleClaimsComponent } from './pages/account/role-claims/role-claims.component';
import { RoleComponent } from './pages/account/role/role.component';
import { UserRoleComponent } from './pages/account/user-role/user-role.component';
import { UserComponent } from './pages/account/user/user.component';
import { IndexComponent } from './pages/dashboard/index/index.component';
import { LayoutComponent } from './pages/dashboard/layout/layout.component';

import { RegisterComponent } from './pages/account/register/register.component';
import { AddPartnerComponent } from './pages/partner/add-partner/add-partner.component';
import { PartnerListComponent } from './pages/partner/partner-list/partner-list.component';
import { FacilityComponent } from './pages/setting/facility/facility.component';
import { RoomTypesComponent } from './pages/setting/room-types/room-types.component';
import { PersonComponent } from './pages/setting/person/person.component';
import { AddPackageComponent } from './pages/setting/package/add-package/add-package.component';
import { PackageListComponent } from './pages/setting/package/package-list/package-list.component';
import { AddAdsComponent } from './pages/ads/add-ads/add-ads.component';
import { AdsListComponent } from './pages/ads/ads-list/ads-list.component';
import { AddFacilityComponent } from './pages/setting/facility/add-facility/add-facility.component';
import { HelpandsupportComponent } from './pages/setting/helpandsupport/helpandsupport.component';
import { CustomerNotificationsComponent } from './pages/setting/customer-notifications/customer-notifications.component';
import { PartnerNotificationsComponent } from './pages/setting/partner-notifications/partner-notifications.component';

const routes: Routes = [
  
    { path: 'login',component: LoginComponent},
    {path: '',component:LayoutComponent,
    children: [
      // Account or settings
      { path: '', component: IndexComponent, pathMatch: 'full' },
      { path: 'home', component: IndexComponent },
      { path: 'users', component: UserComponent },
      { path: 'test', component: RegisterComponent },
      { path: 'roles', component: RoleComponent },
      { path: 'user_roles', component: UserRoleComponent },
      { path: 'role_claims', component: RoleClaimsComponent },

      //Partner
      { path: 'add_partner/:id', component: AddPartnerComponent },
      { path: 'partner_list', component: PartnerListComponent },
      //Setting
      { path: 'facilities', component: FacilityComponent },
      { path: 'add-facility/:id', component: AddFacilityComponent },
      { path: 'room_types', component: RoomTypesComponent },
      { path: 'persons', component: PersonComponent },
      { path: 'add_package/:id', component: AddPackageComponent },
      { path: 'package_list', component: PackageListComponent },
      //ads
      { path: 'add_ads/:id', component: AddAdsComponent },
      { path: 'ads_list', component: AdsListComponent },

      { path: 'helpAndSupport', component: HelpandsupportComponent },
      { path: 'customer_notification', component: CustomerNotificationsComponent },
      { path: 'partner_notification', component: PartnerNotificationsComponent },

    ]
    },
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{ useHash:true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
