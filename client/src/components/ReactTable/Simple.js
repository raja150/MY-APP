
import { default as React } from 'react';
import { Table } from 'reactstrap';
import './style.css';
import _ from 'lodash';
import styled from 'styled-components'
import { useTable, useRowSelect, usePagination, useResizeColumns, useFlexLayout, useSortBy } from 'react-table';


function Simple({ columns, data, ...props }) {

    const TableComponent = styled.div``

    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        page,
        nextPage,
        previousPage,
        canNextPage,
        canPreviousPage,
        state: { pageIndex, pageSize },
        pageOptions,
        gotoPage,
        pageCount,
        allColumns,
        setPageSize,
        prepareRow,

    } = useTable({
        columns: columns,
        data: data,
        initialState: {
            pageIndex: 0,
            pageSize: 10,
        },
    }, useSortBy, usePagination, useResizeColumns, useFlexLayout, useRowSelect)

    return <>
        <TableComponent>
            <Table responsive hover {...getTableProps()} id='table-to-xls'>
                <thead >
                    {headerGroups.map(headerGroup => (
                        <tr {...headerGroup.getHeaderGroupProps()}>
                            {headerGroup.headers.map(column => (
                                <th className={column.alignCSS || ''} {...column.getHeaderProps(column.getSortByToggleProps())} >
                                    {column.render('Header')}
                                    {column.canResize && (
                                        <div {...column.getResizerProps()} className={`resizer`} />
                                    )}
                                    <span>
                                        {column.isSorted ? (column.isSortedDesc ? '▼' : '▲') : ''}
                                    </span>
                                </th>

                            ))}
                        </tr>
                    ))}
                </thead>
                <tbody {...getTableBodyProps()}>
                    {
                        page.length > 0 ? page.map(row => {
                            prepareRow(row)
                            return (
                                <tr {...row.getRowProps()}  >
                                    {
                                        row.cells.map((cell) => {
                                            return <td className={cell.column.alignCSS || ''}
                                                {...cell.getCellProps()}>{cell.render('Cell')}</td>
                                        })
                                    }
                                </tr>
                            )
                        }) : <tr><td className='text-center'> No Data Found</td></tr>
                    }
                    {/* {
                    page.length > 0 ? page.slice(0, 1).map(row => {
                        prepareRow(row)
                        return (
                            <tr {...row.getRowProps()}  >
                                {
                                    row.cells.map((cell, idx) => {
                                        return <td className={"aggregate " + (cell.column.alignCSS || '')}  {...cell.getCellProps()}>{columns[idx].aggregate ? _.sumBy(data, columns[idx].accessor) : ''}</td>
                                    })
                                }
                            </tr>
                        )
                    }) : <tr><td className='text-center'> No Data Found</td></tr>
                } */}
                </tbody>

            </Table>
            <div className='mt-1'>
                <div className='d-flex justify-content-end align-items-center'>
                    <div className='mr-2'>
                        <span>
                            Page{' '}
                            <strong>{pageIndex + 1}of {pageOptions.length}</strong>
                        </span>
                    </div>
                    <div className='mr-2'>
                        <span className='mt-2 font-bold'>
                            | Go to Page:{' '}
                            <input className='mt-2 font-bold' type='number' onChange={(e) => {
                                const pageNumber = e.target.value ? Number(e.target.value) - 1 : 0
                                gotoPage(pageNumber)
                            }} style={{ width: '50px' }} />
                        </span>
                    </div>
                    <div className='mr-2'>
                        <select className='mt-2 font-bold' value={pageSize} onChange={(e) => setPageSize(Number(e.target.value))}>
                            {[5, 10, 20, 30, 50].map(pageSize => (
                                <option key={pageSize} value={pageSize}>
                                    {pageSize}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div>
                        <button className='mt-2 font-bold' onClick={() => gotoPage(0)} disabled={!canPreviousPage}>{'<'}</button>
                        <button className='mt-2 ml-2 font-bold' onClick={() => previousPage()} disabled={!canPreviousPage}>Previous</button>
                        <button className='mt-2 ml-2  font-bold' onClick={() => nextPage()} disabled={!canNextPage}>Next</button>
                        <button className='mt-2 ml-2 font-bold' onClick={() => gotoPage(pageCount - 1)} disabled={!canNextPage}>{'>'}</button>
                    </div>

                </div>
            </div>
        </TableComponent>
    </>
}
export default Simple;