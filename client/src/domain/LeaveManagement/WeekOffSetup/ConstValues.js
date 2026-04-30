import React from 'react'

export function Details() {
    return {
        id: '',
        name: ''
    };
}

export const WeekOffTabs = [
    { id: 0, text: 'Week Off Days' },
    { id: 1, text: 'Location' },
    { id: 2, text: 'Department' },
    { id: 3, text: 'Designation' },
    { id: 4, text: 'Team' },
    { id: 5, text: 'Employee' }
]

export const WeekOffColumns = [
    {
        Header: 'Type',
        accessor: 'type',
        Cell: ({ value }) => (
            <div>{value == 1 ? "Week in a month" : 'Week in a year'}</div>
        )
    },
    {
        Header: 'Week Day',
        accessor: 'weekDay',
        Cell: ({ value }) => (
            <div>{value == 1 ? "Monday" : value == 2 ? 'Tuesday'
                : value == 3 ? 'Wednesday' : value == 4 ? 'Thursday' : value == 5 ? 
                'Friday' : value == 6 ? 'Saturday' : 'Sunday'}</div>
        )
    },
    {
        Header: 'Week',
        accessor: 'button',
        name: 'Edit',
    },
    
]

