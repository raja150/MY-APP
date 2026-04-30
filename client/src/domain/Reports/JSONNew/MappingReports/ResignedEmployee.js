export const ResignedEmployees = {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "ResignedEmployees",
    "label": "Resigned Employees",
    "url": 'LMSReport/ResignedEmployees',
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
            "id" : "425cb45d-5a6d-4cb3-9148-14f820f3e864",
            "name" : "fromDate",
            "label" :"Resigned From Date",
            "field" : "DatePicker",
            "url" : "",
            "type" : "Date",
            "data" : [],
            "orderBy" : 4
        },
        {
            "id":"425cb45d-5a6d-4cb3-9148-14f820f3e864",
            "name" : "toDate",
            "label" : "Resigned To Date",
            "field":"DatePicker",
            "type" : "Date",
            "data" : [],
            "url" : "",
            "orderBy" : 5
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
            "name": "resignationOn",
            "label": "Resignation On",
            "type": "Date",
            "data": [],
            "orderBy": 3,
            "status": true
        },
        {
            "name": "resignationType",
            "label": "Resignation Type",
            "type": "int",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "leavingOn",
            "label": "Leaving On",
            "type": "Date",
            "data": [],
            "orderBy": 5,
            "status": true
        },
        {
            "name": "leavingReason",
            "label": "Leaving Reason",
            "type": "string",
            "data": [],
            "orderBy": 6,
            "status": true
        },
        {
            "name": "allowRehiring",
            "label": "Allow Rehiring",
            "type": "string",
            "data": [],
            "orderBy": 7,
            "status": true
        },
        {
            "name": "description",
            "label": "Description",
            "type": "string",
            "data": [],
            "orderBy": 8,
            "status": true
        }
    ]
}