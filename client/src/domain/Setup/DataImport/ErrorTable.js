import React from 'react';
import { Card, CardBody } from 'reactstrap';
import { useTable } from 'react-table'


function ErrorTable(props) {
    const { columns, data } = props;
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow,
    } = useTable({ columns: columns, data: data })

    return <Card >
        <CardBody>
            <div className='-dataTable'>
                <div className='table table-responsive border'>
                    <table {...getTableProps()} style={{ border: 'solid 1px blue' }}>
                        <thead>
                            {headerGroups.map(headerGroup => (
                                <tr {...headerGroup.getHeaderGroupProps()}>
                                    {headerGroup.headers.map(column => (
                                        <th
                                            {...column.getHeaderProps()}
                                            style={{
                                                borderBottom: 'solid 3px red',
                                                background: 'aliceblue',
                                                color: 'black',
                                                fontWeight: 'bold',
                                                borderRight: 'solid 1px'
                                            }}
                                        >
                                            {column.render('Header')}
                                        </th>
                                    ))}
                                </tr>
                            ))}
                        </thead>
                        <tbody {...getTableBodyProps()}>
                            {rows.map(row => {
                                prepareRow(row)
                                return (
                                    <tr {...row.getRowProps()}>
                                        {row.cells.map(cell => {
                                            return (
                                                <td
                                                    {...cell.getCellProps()}
                                                    style={{
                                                        padding: '10px',
                                                        border: 'solid 1px gray',
                                                        background: 'papayawhip',
                                                    }}
                                                >
                                                    {cell.render('Cell')}
                                                </td>
                                            )
                                        })}
                                    </tr>
                                )
                            })}
                        </tbody>
                    </table>
                </div>
            </div>
        </CardBody>
    </Card>
}
export default ErrorTable;