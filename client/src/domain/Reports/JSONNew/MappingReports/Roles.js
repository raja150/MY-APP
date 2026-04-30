export const Roles= {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "roles",
    "label": "Roles",
    "url": 'LmsReport/PageEmployee',
    "summaryTableUrl": 'LmsReport/PageRole',
    "type": 1,
    "orderBy": 0,
    "filters": [
        {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "moduleId",
            "label": "module",
            "field": "Dropdown",
            "url": "Role/Modules",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 0
         },
          {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "pageId",
            "label": "Page",
            "field": "Dropdown",
            "url": "Role/Pages",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 1
          },
    ],
    "columns": [
        {
            "name": "employeeCode",
            "label": "Employee Code",
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
            "name": "roleName",
            "label": "Role Name",
            "type": "string",
            "data": [],
            "orderBy": 5,
            "status": true
        },
    ],
    "summaryTable": [
        {
            "name": "roleName",
            "label": "Role Name",
            "type": "string",
            "data": [],
            "orderBy": 1,
            "status": true
        },
        {
            "name": "view",
            "label": "View",
            "type": "string",
            "data": [],
            "orderBy": 2,
            "status": true
        },
        {
            "name": "add",
            "label": "Add",
            "type": "string",
            "data": [],
            "orderBy": 3,
            "status": true
        },
        {
            "name": "update",
            "label": "Update",
            "type": "string",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "delete",
            "label": "Delete",
            "type": "string",
            "data": [],
            "orderBy": 5,
            "status": true
        },
    ],
}
