export const LeaveBalance={
    columns:[
        {
            Header:'Employee Code',accessor:'no'
        },
        {
            Header:'Employee Name',accessor:'name'
        },
        {
            Header:'Leaves',accessor:'leaves'
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
                name: "no",
                propName: "No",
                required: true,
                showInList: true,
                showInSearch: false,
                type: "Input"
            },
        ]
    }
}