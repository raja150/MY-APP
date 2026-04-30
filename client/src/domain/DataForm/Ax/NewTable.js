import { faAngleLeft, faAngleRight, faSearchPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { notifyError } from 'components/alert/Toast';
import Loading from 'components/Loader';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { Link, useHistory, useParams } from 'react-router-dom';
import { useFlexLayout, usePagination, useResizeColumns, useRowSelect, useSortBy, useTable } from 'react-table';
import { Breadcrumb, BreadcrumbItem, Button, Card, CardBody } from 'reactstrap';
import SessionStorageService from 'services/SessionStorage';
import { TableComponent } from 'Site_constants';
import * as appUtil from 'utils/app';
import * as formUtil from 'utils/form';
import APIService from '../../../services/apiservice';
import * as crypto from '../../../utils/Crypto';
import SearchPopUp from './SearchPopUp';


function NewTable(props) {

    let { id } = useParams();
    const [formId, setFormIDData] = useState('');
    const [searchData, setSearchData] = useState({})
    let size = SessionStorageService.getPageSize();
    const [loading, setLoading] = useState(false)
    const [state, setFrmState] = useState({
        module: '', entityName: '', columns: [], isLoading: true,
        data: [], pages: -1, title: '', pageSize: size ? size : 10,
        hasNext: false, hasPrevious: false, pageIndex: 0, searchFields: [], count: 0
    })
    const [modal, setModal] = useState(false)
    const toggle = () => { setModal(!modal) }
    let queryProps = { ...queryString.parse(props.location.search) };

    //On route change update formId
    if (id !== formId) {
        setFormIDData(id);
        setSearchData({});
    }

    const handleEdit = (n) => {
        const qry = { q: formId, r: (n.id ? crypto.encrypt(n.id) : '') };
        props.history.push('/d/screen/?' + queryString.stringify(qry));
    }
    const history = useHistory();


    useEffect(() => {
        //Component re-initialization won't be happen when same component recalled 
        //So, isLoading is will be false  
        const fetchData = async () => {
            const result = await APIService.getAsync('appForms/' + formId).catch((error) => {
                notifyError(error.message);
            });
            if (result) {
                // const listResult = await APIService.getAsync(result.data.name);
                const json = JSON.parse(result.data.json);
                const firstTab = json.tabs[0];
                const advanceSearch = result.data.searchJSON ? JSON.parse(result.data.searchJSON) : [];
                const fields = formUtil.getFieldsFromFieldGroup(firstTab);
                const searchFields = advanceSearch ? advanceSearch : []
                const ddData = await formUtil.getPaginateFieldData(fields);
                setSearchData(queryProps)
                const cols = formUtil.getTableCols(firstTab, ddData);

                const apiData = await getData(json.module, result.data.name, size, queryProps)

                setFrmState({
                    ...state, isLoading: false, columns: cols, data: apiData.data,
                    title: result.data.displayName, entityName: result.data.name, module: json.module,
                    pages: apiData.pages, hasNext: apiData.hasNext,
                    hasPrevious: apiData.hasPrevious, searchFields: searchFields, count: apiData.count,
                    pageIndex: apiData.pageIndex, fields: fields, icon: json.icon
                });
                document.title = appUtil.pageTitle(result.data.displayName);
            }
        };
        fetchData();
    }, [formId]);

    const getData = async (module, name, size, query) => {
        setLoading(true)
        let pages = '', hasNext = false, hasPrevious = false, pageIndex = 0, data = [], count = 0
        const params = new URLSearchParams();

        Object.keys(query).forEach(function (key) {
            params.append(key, query[key])
        });
        history.push({ search: params.toString() });
        const qString = queryString.stringify({
            ...query,
            page: query['page'] || 0,
            size: size || 10,
        });
        await APIService.getAsync(`${module}/${name}/Paginate?${qString}`).then((response) => {
            data = response.data.items;
            pages = response.data.pages;
            hasNext = response.data.hasNext;
            hasPrevious = response.data.hasPrevious;
            pageIndex = response.data.index;
            count = response.data.count
        }).catch((err) => {
            formUtil.displayErrors(err)
        });
        setLoading(false)
        return {
            data: data,
            pages: pages,
            hasNext: hasNext,
            hasPrevious: hasPrevious,
            pageIndex: pageIndex,
            count: count
        }
    }



    const handleOnChange = (name, value) => {
        if (name == 'name') {
            const searchParams = Object.keys(queryProps);
            //setting searched parameters to initial values
            searchParams.forEach((e) => {
                //checking searched param key is exist
                // or not in query props
                if (e in searchData) {
                    //updating the searched value to initial value
                    searchData[e] = ''
                }
            })

        }
        setSearchData({ ...searchData, [name]: value });
    }

    const handleSearch = async (page, pageSize, sortBy, isDescend) => {
        //Push search parameters into router history
        searchData['page'] = page || 0
        searchData['sortBy'] = sortBy ? sortBy : (queryProps['sortBy'] ? queryProps['sortBy'] : 'AddedAt')
        searchData['isDescend'] = isDescend != undefined ? isDescend : (queryProps['isDescend'] ?
            queryProps['isDescend'] : false)

        SessionStorageService.setPageSize(pageSize)

        const apiData = await getData(state.module, state.entityName, pageSize || size, searchData)
        setFrmState({
            ...state, data: apiData.data, pages: apiData.pages, hasNext: apiData.hasNext,
            hasPrevious: apiData.hasPrevious, pageIndex: apiData.pageIndex, count: apiData.count
        });
    }

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
        columns: state.columns,
        data: state.data,
        manualSortBy: true,
        disableSortRemove: true,
        initialState: {
            // pageIndex: state.pageIndex,
            pageSize: state.pageSize,
            hiddenColumns: state.columns.map(column => {
                if (column.show === false) return column.accessor || column.id;
            }),
            sortBy: []
        },

    }, useSortBy, usePagination, useResizeColumns, useFlexLayout, useRowSelect);

    useEffect(() => {
        if (sortBy.length > 0) {
            handleSearch(state.pageIndex, pageSize, sortBy[0].id, sortBy[0].desc);
            setSearchData({ ...searchData, sortBy: sortBy[0].id, isDescend: sortBy[0].desc });
        }
        //Page initial loading entity name is empty so, stop calling API search
        else if (state.entityName) {
            handleSearch(state.pageIndex, pageSize, '', false);
            setSearchData({ ...searchData, sortBy: '', isDescend: false });
        }
    }, [sortBy]);

    const handleAdvanceSearch = () => {
        setModal(true)
        searchData['name'] = ''
        queryProps['page'] = 0
    }

    return (
        <Fragment>
            {state.isLoading ? <Loading /> :
                <ReactCSSTransitionGroup
                    component="div"
                    transitionName="TabsAnimation"
                    transitionAppear={true}
                    transitionAppearTimeout={0}
                    transitionEnter={false}
                    transitionLeave={false}
                    key={state.formJson ? state.formJson.id : ''}>

                    <div className="app-page-title pt-2 pb-2">
                        <div className="page-title-wrapper">
                            <div className="page-title-heading">
                                <Breadcrumb>
                                    <div style={{ fontSize: '30px' }} className={state.icon} />
                                    <BreadcrumbItem> {state.module} </BreadcrumbItem>
                                    <BreadcrumbItem>
                                        {state.title}
                                    </BreadcrumbItem>
                                </Breadcrumb>
                            </div>
                            <div className="page-title-actions">
                                <div className="d-flex">
                                    <div className="input-icon mr-3">
                                        <input name="name" value={searchData['name'] ? searchData['name'] : ''} className="form-control form-control-solid bg-white" placeholder="Search Name"
                                            onChange={(e) => handleOnChange(e.target.name, e.target.value)} onKeyPress={(e) => {
                                                e.key === "Enter" && handleSearch(0, size)
                                            }} />
                                        <span onClick={(e) => handleSearch(0, size)}>
                                            <i className="pe-7s-search text-muted"></i>
                                        </span>
                                    </div>
                                    {state.searchFields.length > 0 ?
                                        <div className="btn-actions-pane-right" onClick={() => handleAdvanceSearch(true)} style={{ marginRight: '10px', marginTop: '5px' }} >
                                            <FontAwesomeIcon icon={faSearchPlus} size="2x" color='DeepSkyBlue' />
                                        </div>
                                        : ''}
                                    <Link to={props.match.path === '/d/list/:id' ? `/d/screen?q=${formId || ''}` : `/m/Nursing/screen/add?q=${formId || ''}`}>
                                        <span href="#" className="btn btn-white btn-hover-primary btn-sm">
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
                                        </span>
                                    </Link></div>
                            </div>
                        </div>
                    </div>
                    <Card>
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
                                        {loading ? <Loading /> :
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
                                        }
                                    </table>
                                </div>

                                <div className='d-flex justify-content-end align-items-center'>
                                    <div style={{ float: 'left', width: '80%', paddingLeft: '5px',color:'green' }}>
                                        Total No.of Records : {state.count}
                                    </div>
                                    <div style={{ float: 'right', width: '20%', margin: '5px' }}>
                                        <select
                                            value={pageSize}
                                            onChange={e => {
                                                setPageSize(Number(e.target.value));
                                                handleSearch(state.pageIndex, Number(e.target.value))
                                            }}
                                        >
                                            {[5, 10, 20, 30, 40, 50].map(pageSize => (
                                                <option key={pageSize} value={pageSize}>
                                                    {pageSize}
                                                </option>
                                            ))}
                                        </select>
                                        <span style={{ padding: '3px' }} >
                                            Page {state.pageIndex + 1} of {state.pages}
                                        </span>
                                        <FontAwesomeIcon className='mr-3 -cursor' icon={faAngleLeft} size='lg' onClick={() => (canPreviousPage || state.hasPrevious) && handleSearch(state.pageIndex - 1, pageSize, sortBy[0]?.id, sortBy[0]?.desc)} disabled={!canPreviousPage} />
                                        <FontAwesomeIcon className='-cursor' icon={faAngleRight} size='lg' onClick={() => (canNextPage || state.hasNext) && handleSearch(state.pageIndex + 1, pageSize, sortBy[0]?.id, sortBy[0]?.desc)} disabled={!canNextPage} />
                                    </div>
                                </div>
                            </TableComponent>
                            <SearchPopUp modal={modal} setModal={setModal} toggle={toggle}
                                handleSearch={handleSearch} handleOnChange={handleOnChange}
                                fields={state.searchFields} queryProps={queryProps}
                            />
                        </CardBody>
                    </Card>
                </ReactCSSTransitionGroup>
            }
        </Fragment>
    )
}

export default NewTable
