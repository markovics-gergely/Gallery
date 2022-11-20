import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { JwtModule } from '@auth0/angular-jwt';
import { RolePipe } from './pipes/role.pipe';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { LoadingComponent } from './components/loading/loading.component';
import { ProfileComponent } from './components/profile/profile.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { MyGalleriesComponent } from './components/my-galleries/my-galleries.component';
import { FavoritesComponent } from './components/favorites/favorites.component';
import { BrowseComponent } from './components/browse/browse.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {MatBadgeModule} from '@angular/material/badge';
import { GalleryComponent } from './components/gallery/gallery.component';
import { PagerComponent } from './components/pager/pager.component';
import { PreviewComponent } from './components/preview/preview.component';
import { ProfileDialogComponent } from './components/dialogs/profile-dialog/profile-dialog.component';
import { AlbumDialogComponent } from './components/dialogs/album-dialog/album-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ConfirmComponent } from './components/dialogs/confirm/confirm.component';
import { MatCheckboxModule } from '@angular/material/checkbox';

@NgModule({
  declarations: [
    AppComponent,
    RolePipe,
    LoginComponent,
    RegisterComponent,
    LoadingComponent,
    ProfileComponent,
    SidebarComponent,
    MyGalleriesComponent,
    FavoritesComponent,
    BrowseComponent,
    GalleryComponent,
    PagerComponent,
    PreviewComponent,
    ProfileDialogComponent,
    AlbumDialogComponent,
    ConfirmComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    NgbModule,
    MatBadgeModule,
    MatDialogModule,
    MatTooltipModule,
    MatCheckboxModule,
    JwtModule.forRoot({
      config: { },
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor, 
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
