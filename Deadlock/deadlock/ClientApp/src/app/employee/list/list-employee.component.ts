import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { fromEvent, Subject, BehaviorSubject, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil, tap } from 'rxjs/operators';
import { ConfirmDeletAlert, SuccessAlert } from 'src/app/common/alert';
import { DataSource } from '@angular/cdk/table';
import { EmployeeService } from '../api/employee.service';
import { employee } from '../employee.model';

@Component({
  selector: 'app-list-employee',
  templateUrl: './list-employee.component.html',
  styleUrls: ['./list-employee.component.scss']
})
export class ListEmployeeComponent implements OnInit {

  dataSource: EmployeesDataSource | null;
  displayedColumns = ["firstName", "lastName", "emplNumber","position","salary","hireDate","buttons"];

  @ViewChild(MatPaginator, { static: true })
  paginator: MatPaginator;


  @ViewChild("filter", { static: true })
  filter: ElementRef;

  private _unsubscribeAll: Subject<any>;

//   /**
//    * constructor
//    */
  constructor(private _employeeService: EmployeeService) {
      this._unsubscribeAll = new Subject();
  }

  ngOnInit(): void {
      this.dataSource = new EmployeesDataSource(this._employeeService);
      this.dataSource.loadEmployee();

      fromEvent(this.filter.nativeElement, "keyup")
          .pipe(
              takeUntil(this._unsubscribeAll),
              debounceTime(150),
              distinctUntilChanged()
          )
          .subscribe(() => {
              if (!this.dataSource) {
                  return;
              }

              this.loadPage();
          });
  }

  ngAfterViewInit(): void {
      this.paginator.page.pipe(tap(() => this.loadPage())).subscribe();
  }

  loadPage() {
      this.dataSource.loadEmployee(
          this.filter.nativeElement.value,
          "asc",
          this.paginator.pageIndex,
          this.paginator.pageSize
      );
  }

  async onDelete(id:string){
    let confirm = await ConfirmDeletAlert();
    if(confirm === true){
        this._employeeService.deleteEmployee(id).then(()=> {
           SuccessAlert("Los datos han sido eliminados correctamente").then(() => this.loadPage())
        })
    }
 }


}

export class EmployeesDataSource extends DataSource<employee> {
  private _employeeSubject = new BehaviorSubject<employee[]>([]);
  private _loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this._loadingSubject.asObservable();

  /**
   *
   */
  constructor(private _employeeService: EmployeeService) {
      super();
  }

  connect(): Observable<any[] | readonly any[]> {
      return this._employeeSubject.asObservable();
  }

  /**
   * Disconnect
   */
  disconnect(): void {
      this._employeeSubject.complete();
      this._loadingSubject.complete();
  }

  loadEmployee(
      filter = "",
      sortDirection = "asc",
      pageIndex = 0,
      pageSize = 10
  ): void {
      this._loadingSubject.next(true);

      this._employeeService
          .getEmployees(pageIndex, pageSize, filter)
          .then(res => {
              this._employeeSubject.next(res);
              this._loadingSubject.next(false);
          })
          .catch(() => {
              this._employeeSubject.next([]);
              this._loadingSubject.next(false);
          });

        
          
  }

  get getTotalEmployee(): number {
      return this._employeeService.total;
  }
}

