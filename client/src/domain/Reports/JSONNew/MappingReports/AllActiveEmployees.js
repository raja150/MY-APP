export const AllActiveEmployees= {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "allActiveEmployees",
    "label": "allActiveEmployees",
    "url": 'LMSReport/AllActiveEmployees',
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
            "name": "fromDate",
            "label": "Date Of Joining From",
            "field": "DatePicker",
            "url": "",
            "type": "Date",
            "data": [],
            "orderBy": 4,
          },
          {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "toDate",
            "label": "Date Of Joining To",
            "field": "DatePicker",
            "url": "",
            "type": "Date",
            "data": [],
            "orderBy": 5
          },
        //   {
        //     "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
        //     "name": "dateOfBirth",
        //     "label": "Date Of Birth",
        //     "field": "DatePicker",
        //     "url": "",
        //     "type": "Date",
        //     "data": [],
        //     "orderBy": 6
        //   },
          {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "workTypeId",
            "label": "Work Type",
            "field": "Dropdown",
            "url": "Organization/WorkType/GetList",
            "type": "int",
            "valueField": "id",
            "textField": "wType",
            "data": [],
            "orderBy": 7
          },
          {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "empCategoryId",
            "label": "Employee Category",
            "field": "Dropdown",
            "url": "Organization/EmployeeCategory/GetList",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 8
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
            "label": "Name",
            "type": "string",
            "data": [],
            "orderBy": 2,
            "status": true
        },
        {
            "name": "department",
            "label": "Department",
            "type": "string",
            "data": [],
            "orderBy": 3,
            "status": true
        },
        {
            "name": "designation",
            "label": "Designation",
            "type": "string",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "team",
            "label": "Team",
            "type": "string",
            "data": [],
            "orderBy": 5,
            "status": true
        },
        {
            "name": "dateOfJoining",
            "label": "DOJ",
            "type": "Date",
            "data": [],
            "orderBy": 6,
            "status": true
        },
        {
            "name": "dateOfBirth",
            "label": "DOB",
            "type": "Date",
            "data": [],
            "orderBy": 7,
            "status": true
        },
        {
            "name": "mobileNumber",
            "label": "Mobile Number",
            "type": "string",
            "data": [],
            "orderBy": 8,
            "status": true
        },
        {
            "name": "workLocation",
            "label": "Work Location",
            "type": "string",
            "data": [],
            "orderBy": 9,
            "status": true
        },
        {
            "name": "workType",
            "label": "Work Type",
            "type": "string",
            "data": [],
            "orderBy": 10,
            "status": true
        },
        {
            "name": "reportingTo",
            "label": "Reporting To",
            "type": "string",
            "data": [],
            "orderBy": 11,
            "status": true
        },
        {
            "name": "workEmail",
            "label": "Work Email",
            "type": "string",
            "data": [],
            "orderBy": 12,
            "status": true
        },
        {
            "name": "personalEmail",
            "label": "Personal Email",
            "type": "string",
            "data": [],
            "orderBy": 13,
            "status": true
        },
        {
            "name": "bloodGroup",
            "label": "Blood Group",
            "type": "string",
            "data": [],
            "orderBy": 14,
            "status": true
        },
        {
            "name": "aadhaarNumber",
            "label": "Aadhaar No",
            "type": "string",
            "data": [],
            "orderBy": 15,
            "status": true
        },
        {
            "name": "panNumber",
            "label": "Pan Number",
            "type": "string",
            "data": [],
            "orderBy": 16,
            "status": true
        },
        {
            "name": "maritalStatusTxt",
            "label": "Marital Status",
            "type": "string",
            "data": [],
            "orderBy": 17,
            "status": true
        },
        {
            "name": "fatherName",
            "label": "Father Name",
            "type": "string",
            "data": [],
            "orderBy": 18,
            "status": true
        },
        {
            "name": "empStatus",
            "label": "Employee Status",
            "type": "string",
            "data": [],
            "orderBy": 19,
            "status": true
        },
        {
            "name": "profileStatusTxt",
            "label": "Profile Status",
            "type": "string",
            "data": [],
            "orderBy": 20,
            "status": true
        },
        {
            "name": "empCategory",
            "label": "Employee Category",
            "type": "string",
            "data": [],
            "orderBy": 21,
            "status": true
        },   
    ]
}
