import { EMPLOYEE_ADDRESS, Employee_Address } from "Site_constants";

export const EmployeeAddress= {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "employeeAddress",
    "label": "Employee Address",
    "url": 'LMSReport/EmployeeActiveAddress',
    "type": 1,
    "orderBy": 0,
    "filters": [
        {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "departmentId",
            "label": "Department",
            "field": "Dropdown",
            "url": "Organization/Department/GetList",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 0
         },
          {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "designationId",
            "label": "Designation",
            "field": "Dropdown",
            "url": "Organization/Designation/GetList",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 1
          },
          {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "teamId",
            "label": "Team",
            "field": "Dropdown",
            "url": "Organization/Team/GetList",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 2
          },
          {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "employeeId",
            "label": "Employee",
            "field": "AsyncDropdown",
            "url": "Search/SearchByEmployee",
            "type": "int",
            "valueField":"id",
            "textField":"name",
            "data": [],
            "orderBy": 3
          },
        {
            "id": "425cb45d-5a6d-4cb3-9148-14f820f3e864",
            "name": "joinDateFrom",
            "label": "Joining Date From",
            "field": "DatePicker",
            "url": "",
            "type": "Date",
            "data": [],
            "orderBy": 4
          },
          {
            "id": "425cb45d-5a6d-4cb3-9148-14f820f3e864",
            "name": "joinDateTo",
            "label": "Joining Date To",
            "field": "DatePicker",
            "url": "",
            "type": "Date",
            "data": [],
            "orderBy": 5
          },
        {
            "name": "type",
            "label": "Address Type",
            "field": "Radio",
            "url": "",
            "type": "int",
            "valueField": "value",
            "textField": "text",
            "data":EMPLOYEE_ADDRESS,
            "orderBy": 6,
            "default":1,
        },
    ],
    "columns": [
      {
        "name": "empName",
        "label": "Employee Name",
        "type": "string",
        "data": [],
        "orderBy": 0,
        "status": true
    },
        {
            "name": "addressLineOne",
            "label": "Address Line 1",
            "type": "string",
            "data": [],
            "orderBy": 1,
            "status": true
        },
        {
            "name": "addressLineTwo",
            "label": "Address Line 2",
            "type": "string",
            "data": [],
            "orderBy": 2,
            "status": true
        },
        {
            "name": "cityOrTown",
            "label": "City/Town",
            "type": "string",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "state",
            "label": "State",
            "type": "Int32",
            "data": [],
            "orderBy": 5,
            "status": true
        },
        {
            "name": "country",
            "label": "Country",
            "type": "Int32",
            "data": [],
            "orderBy": 6,
            "status": true
        },
        
    ]
}
