import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { EmployeeService } from '../api/employee.service';
import { PositionService } from 'src/app/position/api/position.service';
import { position } from 'src/app/position/position.model';
import { Router, ActivatedRoute } from '@angular/router';
import { ErrorAlert, SuccessAlert } from 'src/app/common/alert';
import { Employee, formEmployee, employee } from '../employee.model';

@Component({
  selector: 'app-form-employee',
  templateUrl: './form-employee.component.html',
  styleUrls: ['./form-employee.component.scss']
})
export class FormEmployeeComponent implements OnInit {

  employeeForm: FormGroup;
  positions:position[]= null;
  data:Employee = null;
  id:string;
  constructor(private _fb: FormBuilder,private _employeeService:EmployeeService,private _route:ActivatedRoute, private _positionService:PositionService,private _router:Router) 
  {
  
   }

  async ngOnInit() {
    this.id = this._route.snapshot.paramMap.get('id');
   console.log(this.id);
   
    if(this.id =='new'){
      this.data= new Employee();
    }else{
      await this._employeeService.GetByIdEmployee(this.id).then((res:Employee) => {
        this.data = res;
      })
    }
   
    this.initForm(this.data);
  
    await this._positionService.getAllPositions().then((res:position[]) => {
        if(res.length == 0){
          ErrorAlert('¡porque no se ha encontrado posiciones!',);  
          this._router.navigate(["/employee"])
        }
        this.positions = res
        
      });
  };



  initForm(data){
    this.employeeForm = this._fb.group({
      id:[data.id], 
      positionId:[data.positionId],
      firstName:[data.firstName,Validators.required],
      lastName:[data.lastName,Validators.required],
      gender:[data.gender,Validators.required],
      dateOfBirth:[data.dateOfBirth,Validators.required],
      email:[data.email,Validators.required],
      phoneNumber:[data.phoneNumber,Validators.required],
      mobilePhone:[data.mobilePhone,Validators.required],
      streetName:[data.streetName,Validators.required],
      houseNumber:[data.houseNumber,Validators.required],
      municipality:[data.municipality,Validators.required],
      sector:[data.sector,Validators.required],
      city:[data.city,Validators.required],
      country:[data.country,Validators.required],
      emplNumber:[data.emplNumber,Validators.required],
      salary:[data.salary,Validators.required],
      emailEmployee:[data.emailEmployee,Validators.required],
      status:[data.status]
    })
  }

  retunList(){
    this._router.navigate(["/employee"])

  }
  async onSave(){
    const data = this.employeeForm.getRawValue();
    try{
        await this._employeeService.saveEmployee(data).then(() => {
        SuccessAlert("Los datos han sido guardado correctament");
        this._router.navigate(["/employee"])
        });
  
    }catch(err){

      ErrorAlert('¡error al momento de guardar los datos!',);
    }
    
  }

  async onUpdate(){
    let data:formEmployee = this.employeeForm.getRawValue();
    try{
    await this._employeeService.updateEmployee(data).then(() => {
     SuccessAlert( '¡Los datos han sido guardados!',);
     this._router.navigate(["/employee"])
     });
    }catch(err){
      ErrorAlert('¡error al momento de actualizar los datos!',);

    }
  }

}
