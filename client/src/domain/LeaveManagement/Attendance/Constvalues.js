export const LeaveTypes = { 0: "Present", 1: "Absent", 2: "CL", 3: "Week Off", 4: "Half-Day" }

export const UnChangebleColumns = [
    {
        Header: "Employee Name",
        accessor: "name",
        sticky: 'left',
        // Header: () => (
        //     <div className='text-right'>Employee Name</div>
        // )
    },
    {
        Header: "Emp Code",
        accessor: "no",
        sticky: 'left',

    },
    {
        Header: "Designation",
        accessor: "designation",
    },
    {
        Header: "Email",
        accessor: "workEmail",
    },
    {
        Header: "DOJ",
        accessor: "dateOfJoining",
        
    }
]

export const UnChangebleData = [
    { id: 1, empname: 'Anudeep', code: 'code', designation: 'designation', email: 'email', doj: 'doj'},
    { id: 2, empname: 'Mohan', code: 'code', designation: 'designation', email: 'email', doj: 'doj'}
]