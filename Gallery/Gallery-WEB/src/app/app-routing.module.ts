import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BrowseComponent } from './components/browse/browse.component';
import { FavoritesComponent } from './components/favorites/favorites.component';
import { LoginComponent } from './components/login/login.component';
import { MyGalleriesComponent } from './components/my-galleries/my-galleries.component';
import { ProfileComponent } from './components/profile/profile.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth.guard';
import { LoginGuard } from './guards/login.guard';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [LoginGuard],
  },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: []
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'mygalleries',
    component: MyGalleriesComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'favorites',
    component: FavoritesComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'browse',
    component: BrowseComponent,
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    redirectTo: 'login',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AppRoutingModule { }