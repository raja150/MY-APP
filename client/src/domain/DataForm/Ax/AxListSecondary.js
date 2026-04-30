import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import Loader from 'react-loaders';
import ReactTable from 'react-table-6';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import APIService from '../../../services/apiservice';
import * as formUtil from '../../../utils/form';
import AxWizard from './AxWizard';
import * as compare from '../../../utils/Compare';
import { useFlexLayout, usePagination, useResizeColumns, useRowSelect, useSortBy, useTable } from 'react-table';
import { TableComponent } from 'Site_constants';
import { faAngleLeft, faAngleRight, faSearchPlus, faWarehouse } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Loading from 'components/Loader';

function AxListSecondary(props) {
    const { tabJson, refEntityId, module } = props;
    const [wizard, setWizard] = useState(false);
    const [editEntityId, setEditEntityId] = useState('');
    const [searchData, setSearchData] = useState({ refId: refEntityId })
    const [tableLoading, setTableLoading] = useState(false);
    const [state, setFrmState] = useState({
        module: '', entityName: '', isLoading: true, pageIndex: 0,
        columns: [], data: [], pages: -1, title: '', pageSize: 10
    })



    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        const fields = formUtil.getFieldsFromFieldGroup(tabJson);
        const ddData = await formUtil.getFromFieldData(fields);
        const cols = formUtil.getTableCols(tabJson, ddData);
        let pages = '', hasNext = false, hasPrevious = false, pageIndex = 0
        const qString = queryString.stringify({ ...searchData, page: 0, size: 10 });
        await APIService.getAsync(`${module}/${tabJson.name}/Paginate?${qString}`)
            .then((response) => {
                if (response.data.items.length > 0) {
                    const editId = response.data.items[0].id;
                    setEditEntityId(editId ? editId : '')
                    hasPrevious = response.data.hasPrevious;
                    hasNext = response.data.hasNext;
                    pageIndex = response.data.index;
                    pages = response.data.pages;
                }
                setFrmState({
                    isLoading: false, columns: cols, data: response.data.items,
                    pages: pages, hasNext: hasNext,
                    hasPrevious: hasPrevious, pageIndex: pageIndex,
                });
            });
    };
    const handleSearch = async (page, pageSize, sortBy, isDescend) => {
        const qString = queryString.stringify({
            ...searchData, page,
            size: pageSize || state.pageSize,
            sortBy: sortBy || '', isDescend: isDescend || 'false'
        });
        setTableLoading(true);
        await APIService.getAsync(`${module}/${tabJson.name}/Paginate?${qString}`).then((response) => {
            setFrmState({
                ...state, pages: response.data.pages, data: response.data.items, pageSize: pageSize || state.pageSize,
                hasNext: response.data.hasNext, hasPrevious: response.data.hasPrevious, pageIndex: response.data.index
            });
        });
        setTableLoading(false);
    }

    const handleEdit = (n) => {
        setEditEntityId(n.id);
        setWizard(true);
    }
    const handleAdd = () => {
        setEditEntityId('');
        setWizard(true);
    }

    const refresh = () => {
        fetchData();
        setWizard(false);
    }

    //Cancel button for when form allowed for multiple records 
    const handleCancel = () => {
        setEditEntityId('');
        setWizard(false);
    }

    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        page,
        canNextPage,
        canPreviousPage,
        state: { pageSize },
        setPageSize,
        prepareRow,
    } = useTable({
        columns: state.columns,
        data: state.data,
        initialState: {
            pageSize: state.pageSize,
            hiddenColumns: state.columns.filter(column => column.show === false).map(column =>
                (column.accessor || column.id)),
        },

    }, useSortBy, usePagination, useResizeColumns, useFlexLayout, useRowSelect);
    return (
        state.isLoading ? <Loader type="ball-grid-pulse" /> :
            <Fragment>
                {compare.isEqual(tabJson.allowMultiRecords, false) || compare.isEqual(wizard, true) ?
                    <AxWizard  {...{ refEntityId, editEntityId, tabJson, module, ...props }}
                        handleFromSaved={refresh} handleCancel={tabJson.allowMultiRecords ? handleCancel : undefined} />
                    :
                    <Fragment>
                        <Row>
                            <Col md="12">
                                <Button onClick={handleAdd} className="mb-2 mr-2 btn-icon float-right btn-xs btn-primary1" color="primary">
                                    <i className="pe-7s-plus btn-icon-wrapper font-weight-bold" />
                                    ADD
                                </Button>
                            </Col>
                        </Row>
                        <Col md="12" className="p-0">
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
                                                            tableLoading ? <Loading /> :
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
                                                                handleSearch(0, Number(e.target.value))
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
                                                            Page {state.pageIndex + 1} of {state.pages}
                                                        </span>
                                                    </div>
                                                    <div>
                                                        <FontAwesomeIcon className='mr-3 -cursor' icon={faAngleLeft} size='lg' onClick={() => (canPreviousPage || state.hasPrevious) && handleSearch(state.pageIndex - 1, pageSize)} disabled={!canPreviousPage} />
                                                        <FontAwesomeIcon className='-cursor' icon={faAngleRight} size='lg' onClick={() => (canNextPage || state.hasNext) && handleSearch(state.pageIndex + 1, pageSize)} disabled={!canNextPage} />
                                                    </div>

                                                </div>

                                            </div>
                                        </TableComponent>
                                    </CardBody>
                                </Card>
                            </Col>
                        </Col>
                    </Fragment>
                }
            </Fragment>
    );
}
export default AxListSecondary;
