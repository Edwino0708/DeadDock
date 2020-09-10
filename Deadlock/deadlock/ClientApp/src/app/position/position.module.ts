import { NgModule } from '@angular/core';
import {  Routes, RouterModule } from '@angular/router';
import { FormPositionComponent } from './form/form-position.component';
import { ListPositionComponent } from './list/list-position.component';
import { PositionService } from './api/position.service';
//Angular Material
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatSelectModule } from "@angular/material/select";
import { MatSortModule } from "@angular/material/sort";
import { MatTableModule } from "@angular/material/table";
import { MatTabsModule } from "@angular/material/tabs";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatMenuModule } from "@angular/material/menu";
import { FormGroup, FormGroupName, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

const router : Routes= [

  { path: ':id', component:  FormPositionComponent},

  { path: '', component:  ListPositionComponent},
]

@NgModule({
  imports: [
    CommonModule,
    //FlexLayoutModule,
    ReactiveFormsModule,
    RouterModule.forChild(router),    
    MatMenuModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatSelectModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatProgressSpinnerModule,
  ],
  declarations: [FormPositionComponent,ListPositionComponent],
  providers: [PositionService]
  
})
export class PositionModule { }
