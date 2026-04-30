import { RESIGNATION_TYPE } from "Site_constants";

export const ResignedDepartmentEmployees = {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "ResignedDepartmentEmployees",
    "label": "Resigned Department Employees",
    "url": 'LMSReport/ResignedDepartmentEmployees',
    "type": 1,
    "orderBy": 0,
    "filters": [
        {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "lineOfBusinessId",
            "label": "Line of business",
            "field": "Dropdown",
            "url": "Organization/LOB/GetList?OrderBy=name",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 1
        }, 
        {
            "id" : "425cb45d-5a6d-4cb3-9148-14f820f3e864",
            "name" : "fromDate",
            "label" :"Resigned From Date",
            "field" : "DatePicker",
            "url" : "",
            "type" : "Date",
            "data" : [],
            "orderBy" : 2
        },
        {
            "id":"425cb45d-5a6d-4cb3-9148-14f820f3e864",
            "name" : "toDate",
            "label" : "Resigned To Date",
            "field":"DatePicker",
            "type" : "Date",
            "data" : [],
            "url" : "",
            "orderBy" : 3
        }
    ],
    "columns": [

        {
            "name": "employeeNo",
            "label": "Employee No",
            "type": "string",
            "data": [],
            "orderBy": 1,
            "status": true
        },
        {
            "name": "employeeName",
            "label": "Employee Name",
            "type": "string",
            "data": [],
            "orderBy": 2,
            "status": true
        },
        {
            "name": "lob",
            "label": "Line Of Business",
            "type": "string",
            "data": [],
            "orderBy": 3,
            "status": true
        },
        {
            "name": "functionalArea",
            "label": "Functional Area",
            "type": "string",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "designation",
            "label": "Designation",
            "type": "string",
            "data": [],
            "orderBy": 5,
            "status": true
        },
        {
            "name": "dateOfJoining",
            "label": "Joining Date",
            "type": "Date",
            "data": [],
            "orderBy": 6,
            "status": true
        },
        {
            "name": "lastWorkingDate",
            "label": "Last Working Date",
            "type": "Date",
            "data": [],
            "orderBy": 7,
            "status": true
        },
    ]
}