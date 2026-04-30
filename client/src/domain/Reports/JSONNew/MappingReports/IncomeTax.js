export const IncomeTax= {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "IncomeTax",
    "label": "IncomeTax",
    "url": 'PayRollReport/IncomeTax',
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
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "payMonthAndYear",
            "label": "Pay Month And Year",
            "field": "MonthYearPicker",
            "type": "Date",
            "editFormat" : "MM/YY",
            "format":"MM/YY",
            "data": [],
            "orderBy": 4,
            "showDate":1,
            "showTime":0,
            "views": ["year", "decade", "century"],
          },
    ],
    "columns": [
        {
            "name": "empCode",
            "label": "Employee No",
            "type": "string",
            "data": [],
            "orderBy": 1,
            "status": true
        },
        {
            "name": "name",
            "label": "Employee Name ",
            "type": "string",
            "data": [],
            "orderBy": 2,
            "status": true
        },
        {
            "name": "designation",
            "label": "Designation",
            "type": "string",
            "data": [],
            "orderBy": 3,
            "status": true
        },
        {
            "name": "department",
            "label": "Department",
            "type": "string",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "month",
            "label": "Month",
            "type": "string",
            "data": [],
            "orderBy": 5,
            "status": true
        },
        {
            "name": "year",
            "label": "Year ",
            "type": "string",
            "data": [],
            "orderBy": 6,
            "status": true
        },
        {
            "name": "taxAmount",
            "label": "TaxAmount ",
            "type": "string",
            "data": [],
            "orderBy": 7,
            "status": true
        },
        {
            "name": "pan",
            "label": "Pan Number",
            "type": "string",
            "data": [],
            "orderBy": 8,
            "status": true
        },
    ]
}
