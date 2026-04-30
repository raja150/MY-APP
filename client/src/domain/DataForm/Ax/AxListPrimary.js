import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import Loader from 'react-loaders';
import { Link, useParams } from 'react-router-dom';
import ReactTable from 'react-table-6';
import 'react-table-6/react-table.css';
import { Slide, toast } from 'react-toastify';
import { Card, CardBody, Col, Row } from 'reactstrap';
import APIService from '../../../services/apiservice';
import * as crypto from '../../../utils/Crypto';
import * as formUtil from '../../../utils/form';

function AxListPrimary(props) {

    let { id } = useParams();
    
    const [formId, setFormIDData] = useState('');
    // const parsed = queryString.parse(props.location.search);
    const [searchData, setSearchData] = useState({})
    const [state, setFrmState] = useState({ module: '', entityName: '', isLoading: true, columns: [], data: [], pages: -1, title: '', pageSize: 10 })
    const [tableLoading, setTableLoading] = useState(true)

    //On route change update formid
    
    if (id !== formId) {
        setFormIDData(id);
        setSearchData({});
    }

    const handleEdit = (n) => {
        const qry = { q: formId, r: (n.id ? crypto.encrypt(n.id) : '') };
        props.history.push('/d/screen/?' + queryString.stringify(qry));
    }
    const notifyError = (msg) => toast(msg, {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'error'
    });

    useEffect(() => {
        
        //Component re-initialization won't be happen when same component recalled 
        //So, isLoading is will be false  
        setFrmState({ ...state, isLoading: true });
        const fetchData = async () => {
            const result = await APIService.getAsync('appforms/' + formId).catch((error) => {
                notifyError(error.message);
            });
            if (result) {

                // const listresult = await APIService.getAsync(result.data.name);
                const json = JSON.parse(result.data.json);
                const firstTab = json.tabs[0];
                const fields = formUtil.getFieldsFromFieldGroup(firstTab);
                
                const ddData = await formUtil.getFromFieldData(fields);

                const cols = formUtil.getTableCols(firstTab, ddData);
               
                setFrmState({
                    ...state, isLoading: false, data: [], columns: cols, title: result.data.displayName,
                    entityName: result.data.name, module: json.module
                });
            }
        };
        fetchData();
    }, [formId]);

    const handleOnChange = (name, value) => {
        setSearchData({ ...searchData, [name]: value });
    }

    const handleSearch = async (page, pageSize, sortBy, isDescend) => {
        const qstring = queryString.stringify({
            ...searchData, page,
            size: pageSize || state.pageSize,
            sortBy: sortBy || '', isDescend: isDescend || 'false'
        });
        setTableLoading(true);
        await APIService.getAsync(`${state.module}/${state.entityName}/Paginate?${qstring}`).then((response) => {
            setFrmState({ ...state, pages: response.data.pages, data: response.data.items, pageSize: pageSize || state.pageSize });
        });
        setTableLoading(false);

    }

    return (
        state.isLoading ? <Loader type="ball-grid-pulse" /> :
            <Fragment>
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
                                <div>{state.title}</div>
                            </div>
                            <div className="page-title-actions">
                                <div className="d-flex">
                                    <div className="input-icon mr-3">
                                        <input name="name" className="form-control form-control-solid bg-white" placeholder="Search Name" onChange={(e) => handleOnChange(e.target.name, e.target.value)} />
                                        <span onClick={(e) => handleSearch(0)}>
                                            <i className="pe-7s-search text-muted"></i>
                                        </span>
                                    </div>
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
                    <Row>
                        <Col md="12">
                            <Card className="main-card mb-2">
                                <CardBody>
                                    {/* <DataTable state={state} handleSearch={handleSearch} handleEdit={handleEdit} tableLoading={tableLoading} /> */}
                                    <ReactTable
                                        columns={state.columns}
                                        pageSize={state.pageSize}
                                        className="-striped -highlight"
                                        manual // Forces table not to paginate or sort automatically, so we can handle it server-side
                                        data={state.data}
                                        pages={state.pages} // Display the total number of pages
                                        loading={tableLoading} // Display the loading overlay when we need it
                                        onFetchData={async (state, instance) => { 
                                            if (state.sorted.length > 0) {
                                                handleSearch(state.page, state.pageSize, state.sorted[0].id, state.sorted[0].desc);
                                            } else {
                                                handleSearch(state.page, state.pageSize);
                                            }
                                        }}

                                        getTrProps={(state, row) => ({
                                            onClick: () => { handleEdit(row.original) }
                                        })}

                                    >
                                        {(state, makeTable, instance) => {
                                            return makeTable();
                                        }}
                                    </ReactTable>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </ReactCSSTransitionGroup>
            </Fragment>
    );
}

export default AxListPrimary;
