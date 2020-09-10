import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

const routes : Routes = [
  {
    path:"",component:HomeComponent,pathMatch:"full"
  },
  {
    path: "employee",   loadChildren: () => import('./employee/employee.module').then(m => m.EmployeeModule)
  },
   {
    path: "position",   loadChildren: () => import('./position/position.module').then(m => m.PositionModule)
  }
]

@NgModule({
  declarations: [		
    AppComponent,
    NavMenuComponent,
    HomeComponent,
   ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(
      routes
    ),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
