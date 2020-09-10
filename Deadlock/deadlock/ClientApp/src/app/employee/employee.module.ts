import { NgModule } from '@angular/core';
import {  Routes, RouterModule } from '@angular/router';
import { FormEmployeeComponent } from './form/form-employee.component';
import { ListEmployeeComponent } from './list/list-employee.component';
import { EmployeeService } from './api/employee.service';
import { ReactiveFormsModule } from '@angular/forms';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { PositionService } from '../position/api/position.service';

const router : Routes= [

  { path: '', component: ListEmployeeComponent },
  { path: ':id', component: FormEmployeeComponent },
]

@NgModule({
  imports: [
    RouterModule.forChild(router),
    CommonModule,
    //FlexLayoutModule,
    ReactiveFormsModule,
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
  declarations: [FormEmployeeComponent,ListEmployeeComponent],
  providers:[EmployeeService,PositionService]
})
export class EmployeeModule { }
