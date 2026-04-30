import { faAngleLeft, faAngleRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Loading from 'components/Loader';
import LMApproveLeave from 'domain/LeaveManagement/Leaves/Approve';
import React, { Fragment, useEffect } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { Link } from 'react-router-dom';
import { useFlexLayout, usePagination, useResizeColumns, useRowSelect, useSortBy, useTable } from 'react-table';
import 'react-table-6/react-table.css';
import { Card, CardBody, Col, Modal, ModalBody, ModalHeader, Row } from 'reactstrap';
import { TableComponent } from 'Site_constants';
import CompensatoryWork from '../../domain/Approvals/ApplyCompensatory';
import ClientVisit from '../../domain/Approvals/ClientVisit';
import Ticket from '../../domain/Approvals/Ticket';
import Leave from '../../domain/LeaveManagement/LeaveApprovals/Approve';
import LMApproveWFH from 'domain/LeaveManagement/ApplyWFH/Approve';
import ApproveWFH from 'domain/Approvals/WFH/Approval';
import LMClientVisit from 'domain/LeaveManagement/ApplyClientVisits/Approve';
import ApproveClientVisit from 'domain/Approvals/ClientVisit/Approval';
import WebAttendance from 'domain/Approvals/WebAttendance';

const fieldMap = {
    WFH: LMApproveWFH,
    WFHApprove: ApproveWFH,
    ClientVisit: ApproveClientVisit,
    //ClientVisit: ClientVisit,
    Leave: Leave,
    CompensatoryWork: CompensatoryWork,
    Ticket: Ticket,
    Approve: LMApproveLeave,
    ClientVisits: LMClientVisit,
    WebAttendance : WebAttendance
}

function ListPrimary(props) {
    const Component = fieldMap[props.type];
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
        columns: props.state.columns,
        data: props.state.data,
        manualSortBy: true,
        disableSortRemove: true,
        initialState: {
            // pageIndex: state.pageIndex,
            pageSize: props.state.pageSize,
            hiddenColumns: props.state.columns.map(column => {
                if (column.show === false) return column.accessor || column.id;
            }),
        },

    }, useSortBy, usePagination, useResizeColumns, useFlexLayout, useRowSelect)

    useEffect(() => {
        if (sortBy.length > 0) {
            props.handleSearch(props.state.pageIndex, pageSize, sortBy[0].id, sortBy[0].desc);
        }
    }, [sortBy]);
    return (

        props.state.isLoading ? <Loading /> :
            <Fragment>
                <ReactCSSTransitionGroup
                    component="div"
                    transitionName="TabsAnimation"
                    transitionAppear={true}
                    transitionAppearTimeout={0}
                    transitionEnter={false}
                    transitionLeave={false}
                    key={props.state.formJson ? props.state.formJson.id : ''}>

                    <div className="app-page-title pt-2 pb-2 d-md-none d-lg-block">
                        <div className="page-title-wrapper">
                            <div className="page-title-heading">
                                <div>{props.state.title}</div>
                            </div>
                            {props.hideAdd ? '' :
                                <div className="page-title-actions">
                                    <Link to={`${props.match.url}/New`}>
                                        <div className="btn btn-white btn-hover-primary btn-sm">
                                            <span className="svg-icon svg-icon-success svg-icon-lg mr-1">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px">
                                                    <g stroke="none" strokeWidth="1" fill="none" fillRule="evenodd">
                                                        <polygon points="0 0 24 0 24 24 0 24"></polygon>
                                                        <path d="M18,8 L16,8 C15.4477153,8 15,7.55228475 15,7 C15,6.44771525 15.4477153,6 16,6 L18,6 L18,4 C18,3.44771525 18.4477153,3 19,3 C19.5522847,3 20,3.44771525 20,4 L20,6 L22,6 C22.5522847,6 23,6.44771525 23,7 C23,7.55228475 22.5522847,8 22,8 L20,8 L20,10 C20,10.5522847 19.5522847,11 19,11 C18.4477153,11 18,10.5522847 18,10 L18,8 Z M9,11 C6.790861,11 5,9.209139 5,7 C5,4.790861 6.790861,3 9,3 C11.209139,3 13,4.790861 13,7 C13,9.209139 11.209139,11 9,11 Z" fill="#000000" fillRule="nonzero" opacity="0.3"></path>
                                                        <path d="M0.00065168429,20.1992055 C0.388258525,15.4265159 4.26191235,13 8.98334134,13 C13.7712164,13 17.7048837,15.2931929 17.9979143,20.2 C18.0095879,20.3954741 17.9979143,21 17.2466999,21 C13.541124,21 8.03472472,21 0.727502227,21 C0.476712155,21 -0.0204617505,20.45918 0.00065168429,20.1992055 Z" fill="#000000" fillRule="nonzero"></path>
                                                    </g>
                                                </svg>
                                            </span>
                                            Add New
                                        </div>
                                    </Link>
                                </div>}
                        </div>
                    </div>

                    {props.searchCard()}

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
                                                {props.tableLoading ? <Loading /> :
                                                    <tbody {...getTableBodyProps()}>
                                                        {
                                                            page.length > 0 ? page.map(row => {
                                                                prepareRow(row)
                                                                return (
                                                                    <tr {...row.getRowProps()} onClick={() => { props.handleEdit(row.original) }} >
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
                                                }
                                            </table>
                                        </div>
                                        <div className='d-flex justify-content-end align-items-center'>
                                            <div style={{ float: 'left', width: '80%', paddingLeft: '5px',color:'green' }}>
                                                Total No.of Records : {props.state.count}
                                            </div>
                                            <div style={{ float: 'right', width: '20%', margin: '5px' }}>
                                                <select
                                                    value={pageSize}
                                                    onChange={e => {
                                                        setPageSize(Number(e.target.value));
                                                        props.handleSearch(props.state.pageIndex, Number(e.target.value));
                                                    }}
                                                >
                                                    {[5, 10, 20, 30, 40, 50].map(pageSize => (
                                                        <option key={pageSize} value={pageSize}>
                                                            {pageSize}
                                                        </option>
                                                    ))}
                                                </select>
                                                <span style={{ padding: '3px' }}>
                                                    Page {props.state.pageIndex + 1} of {props.state.pages}
                                                </span>
                                                <FontAwesomeIcon className='mr-3 -cursor' icon={faAngleLeft} size='lg' onClick={() => (canPreviousPage || props.state.hasPrevious) && props.handleSearch(props.state.pageIndex - 1, pageSize)} disabled={!canPreviousPage} />
                                                <FontAwesomeIcon className='-cursor' icon={faAngleRight} size='lg' onClick={() => (canNextPage || props.state.hasNext) && props.handleSearch(props.state.pageIndex + 1, pageSize)} disabled={!canNextPage} />
                                            </div>
                                        </div>
                                    </TableComponent>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                    <Modal isOpen={props.modal} toggle={props.toggle} >
                        <ModalHeader>{props.type}</ModalHeader>
                        <ModalBody>
                            {props.type ? <Component rid={props.modalData.id} toggle={props.toggle}
                                state={props.modalData} handleSearch={props.handleSearch} /> : ''}
                        </ModalBody>

                    </Modal>
                </ReactCSSTransitionGroup>

            </Fragment >
    );
}

export default ListPrimary;
