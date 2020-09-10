import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PositionService } from '../api/position.service';
import {  ActivatedRoute, Router } from '@angular/router';
import { Position } from '../position.model';
import { SuccessAlert, ErrorAlert } from 'src/app/common/alert';

@Component({
  selector: 'app-form-position',
  templateUrl: './form-position.component.html',
  styleUrls: ['./form-position.component.scss']
})
export class FormPositionComponent implements OnInit {
  positionForm: FormGroup;
  data:Position= null;
  id:string="";
  constructor(private _fb: FormBuilder,private _positionService:PositionService,private _route:ActivatedRoute,private _router:Router) 
  { 
    
  }

  async ngOnInit() {    
    this.id = this._route.snapshot.paramMap.get('id');
   
    if(this.id =='new'){
      this.data= new Position();
    }else{
      await this._positionService.GetByIdPosition(this.id).then(res => {
        this.data = res;
      })
    }
     
    this.initForm(this.data);
  }

  initForm(data:Position){
    this.positionForm = this._fb.group({
      id:[data.id],
      name:[data.name,Validators.required],
      description:[data.description,Validators.required],
      status:[data.status]
    })
  }


  retunList(){
    this._router.navigate(["/position"])

  }

  async onSave(){
    const data = this.positionForm.getRawValue();
    console.log(data);
    try{
       await this._positionService.savePosition(data).then(() => {
        SuccessAlert( '¡Los datos han sido guardados!',);
        this._router.navigate(["/position"])
        });
  
    }catch(err){

      ErrorAlert('¡error al momento de guardar los datos!',);
    }
    
  }

  async onUpdate(){
    const data = this.positionForm.getRawValue();
    try{
      await this._positionService.updatePosition(data).then(() => {
     SuccessAlert("Los datos han sido actualizado");
     this._router.navigate(["/position"])
     });
    }catch(err){
      ErrorAlert('¡error al momento de actualizar los datos!',);

    }
  }

}
