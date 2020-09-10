import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PagedResponse } from 'src/app/models/paged.response';
import { employee, formEmployee, Employee } from '../employee.model';

@Injectable(
)
export class EmployeeService {
  total: number = 0;
  count: number = 0;
  
private _api_url = "";

constructor(private _http: HttpClient) {
    this._api_url = `${environment.apiServer}`;
}

async getEmployees(page: number, size: number, q: string): Promise<employee[]> {
    try {
        const params = new HttpParams()
            .set("page", page + "")
            .set("size", size + "")
            .set("q", q);
        const res = await this._http
            .get<PagedResponse<employee>>(`${this._api_url}employees`, {
                params
            })
            .toPromise();
        this.count = res.count || 0;
        this.total = res.total || 0;
        return res.data;
    } catch (error) {
        throw error;
    }
}

async GetByIdEmployee(id:string): Promise<Employee> {
    try {
        const res = await this._http
            .get<Employee>(`${this._api_url}employees/${id}`)
            .toPromise();
        return res;
    } catch (error) {
        throw error;
    }
}


async saveEmployee(data: employee): Promise<any> {
  try {
      console.log(data);
      
      await this._http
          .post(`${this._api_url}employees`, data, {
         
          })
          .toPromise();
  } catch (error) {
      throw error;
  }
}

async deleteEmployee(id:string){
  try {
      await this._http
          .delete(`${this._api_url}employees/${id}`)
          .toPromise();
  } catch (error) {
      throw error;
  }
}

async deleteEmployeeStatus(id:string){
  try {
      await this._http
          .delete(`${this._api_url}employees/status/${id}`)
          .toPromise();
  } catch (error) {
      throw error;
  }
}


async updateEmployee(employees: formEmployee): Promise<any> {
    try {
        await this._http
            .put(`${this._api_url}employees`, employees, {
           
            })
            .toPromise();
    } catch (error) {
        throw error;
    }
}
}

