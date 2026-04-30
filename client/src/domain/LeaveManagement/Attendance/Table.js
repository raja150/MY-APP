import { faAngleLeft, faAngleRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect } from 'react';
import { useFlexLayout, usePagination, useResizeColumns, useRowSelect, useSortBy, useTable } from 'react-table';
import { Card, CardBody } from 'reactstrap';

function Table(props) {
  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    page,
    canNextPage,
    canPreviousPage,
    state: { pageSize, sortBy },
    setPageSize,
    prepareRow,
  } = useTable({
    columns: props.columns,
    data: props.data,
    manualSortBy: true,
    disableSortRemove: true,
  }, useSortBy, usePagination, useResizeColumns, useFlexLayout, useRowSelect)

  useEffect(() => {
    if (sortBy.length > 0) {
      props.handleFilterSearch(0, pageSize, sortBy[0].id, sortBy[0].desc);
    }
  }, [sortBy])

  return (
    <Card className="main-card mt-2">
      <CardBody>
        <table className='table table-hover table-responsive' {...getTableProps()}>
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
                  <tr {...row.getRowProps()} >
                    {
                      row.cells.map((cell) => {
                        return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                      })
                    }
                  </tr>
                )
              }) : <tr className='text-center'>No Data Found</tr>
            }
          </tbody>
        </table>
        <div className='mt-1'>
          <div className='d-flex justify-content-end align-items-center'>
            <div className='mr-2 -pageJump'>
              <select
                value={pageSize}
                onChange={e => {
                  setPageSize(Number(e.target.value));
                  props.handleFilterSearch(0, Number(e.target.value));
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
                Page {props.pageIndex + 1} of {props.pages}
              </span>
            </div>
            <div>
              <FontAwesomeIcon className='mr-3 -cursor' icon={faAngleLeft} size='lg' onClick={() => (canPreviousPage || props.hasPrevious) && props.handleFilterSearch(props.pageIndex - 1, pageSize, (sortBy.length > 0 ? sortBy[0].id : ''), (sortBy.length > 0 ? sortBy[0].desc:''))} disabled={!canPreviousPage} />
              <FontAwesomeIcon className='-cursor' icon={faAngleRight} size='lg' onClick={() => (canNextPage || props.hasNext) && props.handleFilterSearch(props.pageIndex + 1, pageSize, (sortBy.length > 0 ? sortBy[0].id : ''), (sortBy.length > 0 ? sortBy[0].desc:''))} disabled={!canNextPage} />
            </div>
          </div>
        </div>
      </CardBody>
    </Card >
  )
}

export default Table
