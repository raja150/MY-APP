export const MainNav = [
    {
        icon: 'ForemostIcons_Outline-01ramak-',
        label: 'Dashboard',
        content: [
            { icon: 'ForemostIcons_Outline-02ramak-' , label: 'Admin', to: '#/Dashboards/Admin' },
            { icon: 'ForemostIcons_Outline-03ramak-' , label: 'Doctor', to: '#/Dashboards/Doctor' },
            { icon: 'ForemostIcons_Outline-04ramak-' , label: 'Reception', to: '#/Dashboards/Reception' },
            { icon: 'ForemostIcons_Outline-05ramak-' , label: 'Biller', to: '#/Dashboards/Biller' },
        ],
    },
    {
        icon: 'ForemostIcons_Outline-06ramak-',
        label: 'Clinic Management',
        content: [
            { icon: 'ForemostIcons_Outline-07ramak-', label: 'Clinic', to: '#/Clinic/list?q=Clinic' },
            { icon: 'ForemostIcons_Outline-08ramak-', label: 'Doctor', to: '#/Clinic/list?q=Doctor' },
            { icon: 'ForemostIcons_Outline-09ramak-', label: 'Clinic Staff', to: '#/Clinic/list?q=Staff' },
            { icon: 'ForemostIcons_Outline-10ramak-', label: 'Staff Shifts', to: '#/Clinic/list?q=StaffShifts' },
            { icon: 'ForemostIcons_Outline-11ramak-', label: 'Staff Atendance', to: '#/Clinic/list?q=StaffAtendance' },
            { icon: 'ForemostIcons_Outline-12ramak-', label: 'Staff Leaves', to: '#/Clinic/list?q=StaffLeaves' },
            { icon: 'ForemostIcons_Outline-13ramak-', label: 'Task Manager', to: '#/Clinic/list?q=Task' },
            { icon: 'ForemostIcons_Outline-14ramak-', label: 'Expense Manager', to: '#/Clinic/list?q=Expenses' },
            { icon: 'ForemostIcons_Outline-15ramak-', label: 'Tickets', to: '#/Clinic/list?q=Ticket' },
            // { label: 'Sample Controls', to: '#/Clinic/list?q=sample' }, 
        ],
    },
    {
        icon: 'ForemostIcons_Outline-16ramak-',
        label: 'Patient Management',
        content: [
            { icon: 'ForemostIcons_Outline-17ramak-', label: 'Patients', to: '#/Patient/All' },
            { icon: 'ForemostIcons_Outline-18ramak-', label: 'Appointment', to: '#/Patient/Appointment' },
            // { label: 'EMR', to: '#/Patient/EMR' },
            // { label: 'Billing', to: '#/Patient/Billing' }
        ],
    },
    {
        icon: 'ForemostIcons_Outline-19ramak-',
        label: 'Pharmacy Management',
        content: [
            { icon: 'ForemostIcons_Outline-20ramak-', label: 'Pharmacy Stock', to: '#/Clinic/list?q=PharmacyStock' },
            { icon: 'ForemostIcons_Outline-21ramak-', label: 'Billing', to: '#/Pharmacy/Billing' },
            { icon: 'ForemostIcons_Outline-22ramak-', label: 'Supplies Inventory', to: '#/Clinic/list?q=Inventory' },
            { icon: 'ForemostIcons_Outline-23ramak-', label: 'Inventory Consume', to: '#/Inventory/Search' },
            { icon: 'ForemostIcons_Outline-24ramak-', label: 'Reports', to: '#/dashboards/list?q=Reports' },
        ],
    },
    {
        icon: 'ForemostIcons_Outline-25ramak-',
        label: 'Settings',
        content: [
            { icon: 'ForemostIcons_Outline-26ramak-', label: 'Inventory Master', to: '#/Clinic/list?q=InventoryItems' },
            { icon: 'ForemostIcons_Outline-27ramak-', label: 'Pharmacy Master', to: '#/Clinic/list?q=PharmacyItems' },
            { icon: 'ForemostIcons_Outline-28ramak-', label: 'Questionnaire ', to: '#/Patient/Questionnaire' },
        ],
    }, 
    // {
    //     icon: 'pe-7s-diamond',
    //     label: 'Pages',
    //     content: [
    //         { label: 'Login', to: '#/pages/login', },
    //         { label: 'Login Boxed', to: '#/pages/login-boxed', },
    //         { label: 'Register', to: '#/pages/register', },
    //         { label: 'Register Boxed', to: '#/pages/register-boxed', },
    //         { label: 'Forgot Password', to: '#/pages/forgot-password', },
    //         { label: 'Forgot Password Boxed', to: '#/pages/forgot-password-boxed', },
    //     ],
    // },
    {
        icon: 'ForemostIcons_Outline-29ramak-',
        label: 'Roles',
        content: [
            { icon: 'ForemostIcons_Outline-30ramak-', label: 'Roles', to: '#/Roles/All', },
        ],
    }
];
