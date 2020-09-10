
export interface employee {
    emplNumber?:string;
    salary?:number;
    status?:boolean;
    emailEmployee?:string;
    supervisor?:string;
    hireDate?:Date;
}

export interface formEmployee{
    id?:string;
    positionId?:string;
    firstName?:string;
    lastName?:string;
    gender?:string;
    dateOfBirth?:Date;
    email?:string;
    phoneNumber?:string;
    mobilePhone?:string;
    streetName?:string;
    houseNumber?:number;
    municipality?:string;
    sector?:string;
    city?:string;
    country?:string;
    emplNumber?:string;
    salary?:number;
    emailEmployee?:string;
    status?:boolean;
}


export class Employee{
    id?:string;
    positionId?:string;
    firstName?:string;
    lastName?:string;
    gender?:string;
    dateOfBirth?:string;
    email?:string;
    phoneNumber?:string;
    mobilePhone?:string;
    streetName?:string;
    houseNumber?:string;
    municipality?:string;
    sector?:string;
    city?:string;
    country?:string;
    emplNumber?:string;
    salary?:string;
    emailEmployee?:string;
    status?:boolean;

    constructor(
        id:string=null,
        positionId:string ="",
        firstName:string="",
        lastName:string = "",
        gender:string = "",
        dateOfBirth:string = "",
        email:string = "",
        phoneNumber:string = "",
        mobilePhone:string = "",
        streetName:string = "",
        houseNumber:string = "",
        municipality:string = "",
        sector:string = "",
        city:string = "",
        country:string = "",
        emplNumber:string = "",
        salary:string = "",
        emailEmployee:string = "",
        status:boolean = false,
    )
    {
        this.id=id;
        this.positionId=positionId;
        this.firstName = firstName
        this.lastName = lastName;
        this.gender = gender;
        this.dateOfBirth = dateOfBirth;
        this.email = email;
        this.phoneNumber = phoneNumber;
        this.mobilePhone = mobilePhone;
        this.streetName = streetName;
        this.houseNumber = houseNumber;
        this.municipality = municipality; 
        this.sector = sector;
        this.city =city;
        this.country =country;
        this.emplNumber = emplNumber;
        this.salary = salary;
        this.emailEmployee = emailEmployee
        this.status =status;
    }
}