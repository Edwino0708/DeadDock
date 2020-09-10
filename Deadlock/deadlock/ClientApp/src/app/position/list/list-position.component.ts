import {
  Component,
  OnInit,
  ViewEncapsulation,
  ViewChild,
  ElementRef,
  AfterViewInit
} from "@angular/core";
import { DataSource } from "@angular/cdk/collections";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";

import { BehaviorSubject, fromEvent, Observable, Subject } from "rxjs";
import {
  debounceTime,
  distinctUntilChanged,
  tap,
  takeUntil
} from "rxjs/operators";
import { position } from "../position.model";
import { PositionService } from "../api/position.service";
import { ConfirmDeletAlert, SuccessAlert } from "src/app/common/alert";

@Component({
  selector: 'app-list-position',
  templateUrl: './list-position.component.html',
  styleUrls: ['./list-position.component.scss']
})
export class ListPositionComponent implements OnInit {

  dataSource: PositionsDataSource | null;
  displayedColumns = ["name", "description", "status","buttons"];

  @ViewChild(MatPaginator, { static: true })
  paginator: MatPaginator;

  @ViewChild("filter", { static: true })
  filter: ElementRef;

  private _unsubscribeAll: Subject<any>;

//   /**
//    * constructor
//    */
  constructor(private _positionService: PositionService) {
      this._unsubscribeAll = new Subject();
  }

  ngOnInit(): void {
      this.dataSource = new PositionsDataSource(this._positionService);
      this.dataSource.loadPosition();

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
      this.dataSource.loadPosition(
          this.filter.nativeElement.value,
          "asc",
          this.paginator.pageIndex,
          this.paginator.pageSize
      );
  }

  async onDelete(id:string){
     let confirm = await ConfirmDeletAlert();
     if(confirm === true){
        await  this._positionService.deletePosition(id).then(()=> {
            SuccessAlert("Los datos han sido eliminados correctamente").then(() => this.loadPage())
         })
     }
  }
}

export class PositionsDataSource extends DataSource<position> {
  private _positionSubject = new BehaviorSubject<position[]>([]);
  private _loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this._loadingSubject.asObservable();

  /**
   *
   */
  constructor(private _positionService: PositionService) {
      super();
  }

  connect(): Observable<any[] | readonly any[]> {
      return this._positionSubject.asObservable();
  }

  /**
   * Disconnect
   */
  disconnect(): void {
      this._positionSubject.complete();
      this._loadingSubject.complete();
  }

  loadPosition(
      filter = "",
      sortDirection = "asc",
      pageIndex = 0,
      pageSize = 10
  ): void {
      this._loadingSubject.next(true);

      this._positionService
          .getPositions(pageIndex, pageSize, filter)
          .then(res => {
              this._positionSubject.next(res);
              this._loadingSubject.next(false);
              console.log(res);
          })
          .catch(() => {
              this._positionSubject.next([]);
              this._loadingSubject.next(false);
          });

        
          
  }

  get getTotalPosition(): number {
      return this._positionService.total;
  }
}

