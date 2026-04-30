import { faAngleLeft, faAngleRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { default as React, Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import Loader from 'react-loaders';
import { Link } from 'react-router-dom';
import { useFlexLayout, usePagination, useResizeColumns, useRowSelect, useSortBy, useTable } from 'react-table';
import { Slide, toast } from 'react-toastify';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import styled from 'styled-components';
import SettingsService from '../../../services/Settings';

const queryString = require('query-string');

const RoleListNew = (props) => {
    const TableComponent = styled.div`
    .table th, .table td {
        padding: 0.35rem 0.55rem !important;
      }
      .table th {
        border-top: none;
        border-bottom: 1px solid #cbcbcb;
      }
     .table td {
        border-top: none;
        border-bottom: 1px solid #e9ecef;
      }
      .-pageJump select {
          width: 50px;
          border: 1px solid #ccc;
          border-radius: 4px;
          padding: 4px;
          margin: 0px 4px;
        }
        .reSizer {
          right: 0;
          /* background: #000; */
          width: 1px;
          height: 100%;
          position: absolute;
          top: 0;
          z-index: 1;
          touch-action :none
        }
        .-cursor{
          cursor: pointer;
        }
        .page-font{
            font-weight: 600
        }
    `

    const parsed = queryString.parse(props.location.search);
    const [roleId, setRoleId] = useState('');
    const [stateData, setStateDate] = useState({ isLoading: true, cols: [], data: [], pages: -1, pageSize: 10 })
    // const cols = [];
    // cols.push({ Header: '', accessor: 'id', show: false, });
    // cols.push({ Header: 'Name', accessor: 'name' });

    //On route change update form id
    if (parsed.q !== roleId) {
        setRoleId(parsed.q);
    }

    const fetchData = async () => {
        let cols = [];
        cols.push({ Header: '', accessor: 'id', show: false, });
        cols.push({ Header: 'Name', accessor: 'name' });

        const result = await SettingsService.getRoleList().catch((error) => {
            notifyError(error.message);
        });

        if (result) {
            setStateDate({ isLoading: false, data: result.data.items, cols: cols });
        }
    };
    const handleEdit = (n) => {
        props.history.push('/m/Settings/Roles/Update?r=' + (n.id ? n.id : ''));
    }
    useEffect(() => {
        fetchData();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);
    const notifyError = (msg) => toast(msg, {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'error'
    });

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
        columns: stateData.cols,
        data: stateData.data,
        initialState: {
            // pageIndex: state.pageIndex,
            pageSize: stateData.pageSize,
            hiddenColumns: stateData.cols.map(column => {
                if (column.show === false) return column.accessor || column.id;
            }),
        },

    }, useSortBy, usePagination, useResizeColumns, useFlexLayout, useRowSelect)

    return (
        stateData.isLoading ? <Loader type="ball-grid-pulse" /> :
            <Fragment>
                <ReactCSSTransitionGroup
                    component="div"
                    transitionName="TabsAnimation"
                    transitionAppear={true}
                    transitionAppearTimeout={0}
                    transitionEnter={false}
                    transitionLeave={false}
                    key='roles'>

                    <div className="app-page-title pt-2 pb-2">
                        <div className="page-title-wrapper">
                            <div className="page-title-heading">
                                <div>Roles</div>
                            </div>
                            <div className="page-title-actions">
                                <Link to={`/m/Settings/Role/Add`}>
                                    <Button className="btn-sm btn-icon btn-primary1" color="primary">
                                        <i className="pe-7s-plus btn-icon-wrapper font-weight-bold"> </i>
                                        ADD
                                    </Button>
                                </Link>
                            </div>
                        </div>
                    </div>


                    <Row>
                        <Col md="12">
                            <Card className="main-card mb-2">
                                <CardBody>
                                    <TableComponent>
                                        <div className='table-responsive'>
                                            <table className='table table-hover' {...getTableProps()}>
                                                <thead >
                                                    {headerGroups.map(headerGroup => (
                                                        <tr {...headerGroup.getHeaderGroupProps()}>
                                                            {headerGroup.headers.map(column => (
                                                                <th {...column.getHeaderProps(column.getSortByToggleProps())} className='border-top-0'>
                                                                    {column.render('Header')}
                                                                    {column.canResize && (
                                                                        <div {...column.getResizerProps()} className={`reSizer`} />
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
                                                                <tr {...row.getRowProps()} onClick={() => { handleEdit(row.original) }} >
                                                                    {
                                                                        row.cells.map((cell) => {
                                                                            return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                                                                        })
                                                                    }
                                                                </tr>
                                                            )
                                                        }) : <tr><td className='text-center'> No Data Found</td></tr>
                                                    }
                                                </tbody>

                                            </table>
                                        </div>
                                        <div className='mt-1'>
                                            <div className='d-flex justify-content-end align-items-center'>
                                                <div className='mr-2 -pageJump'>
                                                    <select
                                                        value={pageSize}
                                                        onChange={e => {
                                                            setPageSize(Number(e.target.value));
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
                                                    <span className='page-font'>
                                                        Page {stateData.pageIndex + 1} of {stateData.pages}

                                                    </span>
                                                </div>
                                                <div>
                                                    <Button className="btn-icon btn-secondary1 rounded-circle mr-1" onClick={() => (canPreviousPage || stateData.hasPrevious) && props.handleSearch(stateData.pageIndex - 1, pageSize)} disabled={!stateData.hasPrevious}>
                                                        <FontAwesomeIcon className='-cursor' icon={faAngleLeft} size='lg' />
                                                    </Button>
                                                    <Button className="btn-icon btn-secondary1 rounded-circle" onClick={() => (canNextPage || stateData.hasNext) && props.handleSearch(stateData.pageIndex + 1, pageSize)} disabled={!stateData.hasNext}>
                                                        <FontAwesomeIcon className='-cursor' icon={faAngleRight} size='lg' />
                                                    </Button>
                                                </div>

                                            </div>

                                        </div>

                                    </TableComponent>

                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </ReactCSSTransitionGroup>
            </Fragment>
    );
};

export default RoleListNew;