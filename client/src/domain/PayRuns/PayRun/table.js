import { faAngleLeft, faAngleRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React,{ Fragment, useEffect } from 'react';
import { useFlexLayout, usePagination, useResizeColumns, useRowSelect, useSortBy, useTable } from 'react-table';
import { Button } from 'reactstrap';
import './style.css';

function Table(props) {
    const { columns, data, pages, hasPrevious, hasNext, showPaginate, } = props;
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        page,
        canNextPage,
        canPreviousPage,
        state: { pageIndex, pageSize, sortBy },
        setPageSize,
        prepareRow,

    } = useTable({
        columns: columns,
        data: data,
        manualSortBy: true,
        disableSortRemove: true,
        initialState: {
            pageIndex: 0,
            pageSize: 10,
            hiddenColumns: columns.length > 0 && columns.map(column => {
                if (column.show === false) return column.accessor || column.id;
            }),
        },

    }, useSortBy, usePagination, useResizeColumns, useFlexLayout, useRowSelect)
    useEffect(() => {
        if (props.handleSearch && sortBy.length > 0) {
            props.handleSearch(0, pageSize, sortBy[0].id, sortBy[0].desc);
        }
    },[sortBy])
    return (
        <div className='-dataTable pb-2'>
            <div className='table-responsive'>

                <table className='table table-bordered table-hover' {...getTableProps()}>
                    <thead >
                        {headerGroups.map(headerGroup => (
                            <tr {...headerGroup.getHeaderGroupProps()}>
                                {headerGroup.headers.map(column => (<th {...column.getHeaderProps(column.getSortByToggleProps())} className='border-top-0'>
                                    {column.render('Header')}
                                    {column.canResize && (
                                        <div {...column.getResizerProps()} className={`resizer`} />
                                    )}
                                    <span>
                                        {column.isSorted ? (column.isSortedDesc ? '▼' : '▲') : ''}
                                    </span>
                                </th>)

                                )}
                            </tr>
                        ))}
                    </thead>
                    <tbody {...getTableBodyProps()}>
                        {
                            page.length > 0 ? page.map(row => {
                                prepareRow(row)
                                return (
                                    <tr {...row.getRowProps()} key={row.id}>
                                        {
                                            row.cells.map((cell) => {
                                                return (
                                                    <Fragment key={cell.column.id}>
                                                        {
                                                            cell.column.id === 'button' ? <td {...cell.getCellProps()}>
                                                                <Button color='primary' onClick={() => props.handleOnEdit(cell.row.original)}>{cell.column.name}</Button></td> :
                                                                <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                                                        }
                                                    </Fragment>
                                                )
                                            })
                                        }
                                    </tr>
                                )
                            }) : <tr><td className='text-center'> No Data Found</td></tr>
                        }
                    </tbody>

                </table>
            </div>
            {
                showPaginate ?
                    <div className='mt-1'>
                        <div className='d-flex justify-content-end align-items-center'>
                            <div className='mr-2 -pageJump'>
                                <select
                                    value={pageSize}
                                    onChange={e => {
                                        setPageSize(Number(e.target.value))
                                        props.handleSearch(0, Number(e.target.value))
                                    }}
                                >
                                    {[5, 10, 20, 30, 40, 50].map(pageSize => (
                                        <option key={pageSize} value={pageSize}>
                                            {pageSize}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div className='mr-2'>
                                <span >
                                    Page {props.pageIndex + 1} of {pages}
                                </span>
                            </div>
                            <div>
                                <FontAwesomeIcon className='mr-3 -cursor' icon={faAngleLeft} size='lg' onClick={() => (canPreviousPage || hasPrevious) && props.handleSearch(props.pageIndex - 1, pageSize)} />
                                <FontAwesomeIcon className='-cursor' icon={faAngleRight} size='lg' onClick={() => (canNextPage || hasNext) && props.handleSearch(props.pageIndex + 1, pageSize)} />
                            </div>

                        </div>

                    </div> : ''
            }
        </div>
    )
}

export default Table
