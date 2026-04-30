
export const Gender = [{ value: 0, text: '' }, { value: 1, text: 'Male' }, { value: 2, text: 'Female' }]
export const MaritalStatus = [{ value: null, text: '' }, { value: 1, text: 'Married' }, { value: 2, text: 'Single' }, { value: 3, text: 'Separated' }]

export const DayPerformance = {
    columns: [
        {
            Header: 'Employee Code', accessor: 'employeeCode'
        },
        {
            Header: 'Perf Date', accessor: 'perfDate'
        },
        {
            Header: 'Target', accessor: 'target'
        },
        {
            Header: 'MC Actual', accessor: 'mcActual'
        },
        {
            Header: 'MC Easy', accessor: 'mcEasy'
        },
        {
            Header: 'MC Medium', accessor: 'mcMedium'
        },
        {
            Header: 'MC Tough', accessor: 'mcTough'
        },
        {
            Header: 'MC VTough', accessor: 'mcvTough'
        },
        {
            Header: 'MC VVery Tough', accessor: 'mcvvTough'
        },
        {
            Header: 'PR Actual', accessor: 'prActual'
        },
        {
            Header: 'PR Easy', accessor: 'prEasy'
        },
        {
            Header: 'PR Medium', accessor: 'prMedium'
        },
        {
            Header: 'PR Tough', accessor: 'prTough'
        },
        {
            Header: 'PR VeryTough', accessor: 'prvTough'
        },
        {
            Header: 'PR VVery Tough', accessor: 'prvvTough'
        },
        {
            Header: 'QC Actual', accessor: 'qcActual'
        },
        {
            Header: 'QC Easy', accessor: 'qcEasy'
        },
        {
            Header: 'QC Medium', accessor: 'qcMedium'
        },
        {
            Header: 'QC Tough', accessor: 'qcTough'
        },
        {
            Header: 'QC Very Tough', accessor: 'qcvTough'
        },
        {
            Header: 'QC VVery Tough', accessor: 'qcvvTough'
        },
        {
            Header: 'AE Actual', accessor: 'aeActual'
        },
        {
            Header: 'AE Easy', accessor: 'aeEasy'
        },
        {
            Header: 'AE Medium', accessor: 'aeMedium'
        },
        {
            Header: 'AE Tough', accessor: 'aeTough'
        },
        {
            Header: 'AE Very Tough', accessor: 'aevTough'
        },
        {
            Header: 'AE VVery Tough', accessor: 'aevvTough'
        },
        {
            Header: 'Performed', accessor: 'performed'
        }

    ],
    searchFields: {
        fields: [
            {
                addValues: false,
                auto: false,
                colSize: 6,
                decimals: false,
                disableDate: false,
                disabled: false,
                displayOrder: 1,
                enableTime: false,
                hidden: false,
                isReference: false,
                jsType: "number",
                label: "Employee Code",
                name: "employeeCode",
                propName: "EmployeeCode",
                required: true,
                showInList: true,
                showInSearch: false,
                type: "Input"
            },
            {
                addValues: false,
                auto: false,
                colSize: 6,
                decimals: false,
                disableDate: false,
                disabled: false,
                displayOrder: 1,
                enableTime: false,
                hidden: false,
                isReference: false,
                jsType: "number",
                label: "Performance Date",
                name: "perfDate",
                propName: "PerfDate",
                required: true,
                showInList: true,
                showInSearch: false,
                type: "RWDatePicker"
            },
        ]
    }
}

export const MonthPerformance = {
    columns: [{
        Header: 'Employee Code', accessor: 'employeeCode'
    },
    {
        Header: 'Perf Month', accessor: 'perfMonth'
    },
    {
        Header: 'Perf Year', accessor: 'perfYear'
    },
    {
        Header: 'Target', accessor: 'target'
    },
    {
        Header: 'MC Actual', accessor: 'mcActual'
    },
    {
        Header: 'MC Easy', accessor: 'mcEasy'
    },
    {
        Header: 'MC Medium', accessor: 'mcMedium'
    },
    {
        Header: 'MCTough', accessor: 'mcTough'
    },
    {
        Header: 'MC VTough', accessor: 'mcvTough'
    },
    {
        Header: 'MC VVery Tough', accessor: 'mcvvTough'
    },
    {
        Header: 'PR Actual', accessor: 'prActual'
    },
    {
        Header: 'PR Easy', accessor: 'prEasy'
    },
    {
        Header: 'PR Medium', accessor: 'prMedium'
    },
    {
        Header: 'PRTough', accessor: 'prTough'
    },
    {
        Header: 'PR Very Tough', accessor: 'prvTough'
    },
    {
        Header: 'PR VVery Tough', accessor: 'prvvTough'
    },
    {
        Header: 'QC Actual', accessor: 'qcActual'
    },
    {
        Header: 'QC Easy', accessor: 'qcEasy'
    },
    {
        Header: 'QC Medium', accessor: 'qcMedium'
    },
    {
        Header: 'QC Tough', accessor: 'qcTough'
    },
    {
        Header: 'QC Very Tough', accessor: 'qcvTough'
    },
    {
        Header: 'QC VVery Tough', accessor: 'qcvvTough'
    },
    {
        Header: 'AE Actual', accessor: 'aeActual'
    },
    {
        Header: 'AE Easy', accessor: 'aeEasy'
    },
    {
        Header: 'AE Medium', accessor: 'aeMedium'
    },
    {
        Header: 'AE Tough', accessor: 'aeTough'
    },
    {
        Header: 'AE VeryTough', accessor: 'aevTough'
    },
    {
        Header: 'AE VVeryTough', accessor: 'aevvTough'
    },
    {
        Header: 'Performed', accessor: 'performed'
    }],
    searchFields: {
        fields: [
            {
                addValues: false,
                auto: false,
                colSize: 6,
                decimals: false,
                disableDate: false,
                disabled: false,
                displayOrder: 1,
                enableTime: false,
                hidden: false,
                isReference: false,
                jsType: "number",
                label: "Employee Code",
                name: "employeeCode",
                propName: "EmployeeCode",
                required: true,
                showInList: true,
                showInSearch: false,
                type: "Input"
            },
            {
                addValues: false,
                auto: false,
                colSize: 6,
                decimals: false,
                disableDate: false,
                disabled: false,
                displayOrder: 1,
                enableTime: false,
                hidden: false,
                isReference: false,
                jsType: "number",
                label: "Performance Month",
                name: "perfMonth",
                propName: "PerfMonth",
                required: true,
                showInList: true,
                showInSearch: false,
                type: "Input"
            },
        ]

    }
}

export const PerformancePayCut = {
    columns: [
        {
            Header: 'Employee Code', accessor: 'employeeCode'
        },
        {
            Header: 'Year', accessor: 'year'
        },
        {
            Header: 'Month', accessor: 'month'
        },
        {
            Header: 'Total Incentive', accessor: 'totalIncentive'
        },
        {
            Header: 'Internal Pay Cut', accessor: 'internalPayCut'
        },
        {
            Header: 'External Pay Cut', accessor: 'externalPayCut'
        },
        {
            Header: 'Total Deduction', accessor: 'totalDeduction'
        },
        {
            Header: 'Production Incentive', accessor: 'productionIncentive'
        },
        {
            Header: 'Night Shift Incentive', accessor: 'nightShiftIncentive'
        },
        {
            Header: 'Spot Incentive', accessor: 'spotIncentive'
        }
        , {
            Header: 'Star Employee Incentive', accessor: 'starEmployeeIncentive'
        },
        {
            Header: 'Other Incentive', accessor: 'otherIncentive'
        }, {
            Header: 'Arrears', accessor: 'arrears'
        },
        {
            Header: 'Unauthorized Leaves', accessor: 'unauthorizedLeaves'
        },
        {
            Header: 'LateComing Deduction', accessor: 'lateComingDeduction'
        },
        {
            Header: 'Amount Not Considered For InternalPay Cut', accessor: 'amountNotConsideredForInternalPayCut'
        },
    ],
    searchFields: {
        fields: [
            {
                addValues: false,
                auto: false,
                colSize: 6,
                decimals: false,
                disableDate: false,
                disabled: false,
                displayOrder: 1,
                enableTime: false,
                hidden: false,
                isReference: false,
                jsType: "number",
                label: "Employee Code",
                name: "employeeCode",
                propName: "EmployeeCode",
                required: true,
                showInList: true,
                showInSearch: false,
                type: "Input"
            },
            {
                addValues: false,
                auto: false,
                colSize: 6,
                decimals: false,
                disableDate: false,
                disabled: false,
                displayOrder: 1,
                enableTime: false,
                hidden: false,
                isReference: false,
                jsType: "number",
                label: "Year",
                name: "year",
                propName: "Year",
                required: true,
                showInList: true,
                showInSearch: false,
                type: "Input"
            }
        ]

    }
}

export const EmployeeProfile = {
    columns: [
        {
            Header: 'No', accessor: 'no'
        },
        {
            Header: 'Name', accessor: 'name',
        },
        {
            Header: 'Designation', accessor: 'designation'
        },
        {
            Header: 'Department', accessor: 'department'
        },
        {
            Header: 'Report To', accessor: 'reportingTo'
        },
        {
            Header: 'Email', accessor: 'personalEmail'
        },
        {
            Header: 'Phone', accessor: 'mobileNumber'
        },
        {
            Header: 'Gender', accessor: 'gender'
        },
        {
            Header: 'Date Of Joining', accessor: 'dateOfJoining'
        },
        {
            Header: 'Date of Birth', accessor: 'dateOfBirth'
        },
        {
            Header: 'Work Type', accessor: 'workType'
        },
        {
            Header: 'Pan Number', accessor: 'panNumber'
        },
        {
            Header: 'Aadhaar Number', accessor: 'aadhaarNumber'
        },
        {
            Header: 'Martial Status', accessor: 'maritalStatus'
        }
    ],
    searchFields: {
        fields: [
            {
            addValues: false,
            auto: false,
            colSize: 6,
            decimals: false,
            disableDate: false,
            disabled: false,
            displayOrder: 1,
            enableTime: false,
            hidden: false,
            isReference: false,
            jsType: "number",
            label: "Employee Code",
            name: "employeeCode",
            propName: "EmployeeCode",
            required: true,
            showInList: true,
            showInSearch: false,
            type: "Input"
        },
        {
            addValues: false,
            auto: false,
            colSize: 6,
            decimals: false,
            disableDate: false,
            disabled: false,
            displayOrder: 1,
            enableTime: false,
            hidden: false,
            isReference: false,
            jsType: "number",
            label: "Department",
            name: "department",
            propName: "DepartmentId",
            required: true,
            showInList: true,
            showInSearch: false,
            type: "RWDropdownList"
        }
    ]

    }
}

