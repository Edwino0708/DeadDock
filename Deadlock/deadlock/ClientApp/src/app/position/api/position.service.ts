import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { position } from '../position.model';
import { PagedResponse } from 'src/app/models/paged.response';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class PositionService {
    total: number = 0;
    count: number = 0;
  private _api_url = "";

  constructor(private _http: HttpClient) {
      this._api_url = `${environment.apiServer}`;
  }

  async getPositions(page: number, size: number, q: string): Promise<position[]> {
      try {
          const params = new HttpParams()
              .set("page", page + "")
              .set("size", size + "")
              .set("q", q);
          const res = await this._http
              .get<PagedResponse<position>>(`${this._api_url}positions`, {
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

  async GetByIdPosition(id:string): Promise<position> {
    try {
        const res = await this._http
            .get<position>(`${this._api_url}positions/${id}`)
            .toPromise();
        return res;
    } catch (error) {
        throw error;
    }
}

  
  async getAllPositions(): Promise<position[]> {
    try {
        const res = await this._http
            .get<position[]>(`${this._api_url}positions/allPositions`)
            .toPromise();

        return res;
    } catch (error) {
        throw error;
    }
}

  async getPosition(id: string): Promise<position> {
      try {
          const res = await this._http
              .get<position>(`${this._api_url}positions/${id}`, {
            
              })
              .toPromise();

          return res;
      } catch (error) {
          throw error;
      }
  }

  async savePosition(data: position): Promise<any> {
    try {
        console.log(data);
        
        await this._http
            .post(`${this._api_url}positions`, data, {
           
            })
            .toPromise();
    } catch (error) {
        throw error;
    }
}

  async updatePosition(position: position): Promise<any> {
      try {
          await this._http
              .put(`${this._api_url}positions`, position, {
             
              })
              .toPromise();
      } catch (error) {
          throw error;
      }
  }

  async deletePosition(id:string){
    try {
        await this._http
            .delete(`${this._api_url}positions/${id}`)
            .toPromise();
    } catch (error) {
        throw error;
    }
  }

  async deletePositionStatus(id:string){
    try {
        await this._http
            .delete(`${this._api_url}positions/status/${id}`)
            .toPromise();
    } catch (error) {
        throw error;
    }
  }
}
