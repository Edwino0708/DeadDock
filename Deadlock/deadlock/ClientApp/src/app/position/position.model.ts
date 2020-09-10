
export interface position {
    id?:string;
    name?:string;
    description?:string;
    status?:boolean;
}

export class Position {
    id?:string;
    name?:string;
    description?:string;
    status?:boolean;

    constructor(
    id:string = null,
    name:string = "",
    description:string = "",
    status:boolean =false
    ) 
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.status = status
    }
}