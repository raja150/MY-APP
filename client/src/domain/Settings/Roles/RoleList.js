import { default as React, Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import Loader from 'react-loaders';
import { Link } from 'react-router-dom';
import ReactTable from 'react-table-6';
import 'react-table-6/react-table.css';
import { Slide, toast } from 'react-toastify';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import SettingsService from '../../../services/Settings';

const queryString = require('query-string');

const RoleList = (props) => {
    const parsed = queryString.parse(props.location.search);
    const [roleId, setRoleId] = useState('');
    const [stateData, setStateDate] = useState({ isLoading: true, data: null })
    const cols = [];
    cols.push({ Header: '', accessor: 'id', show: false, });
    cols.push({ Header: 'Name', accessor: 'name' });

    //On route change update formid
    if (parsed.q !== roleId) {
        setRoleId(parsed.q);
    }

    const fetchData = async () => {
        const result = await SettingsService.getRoleList().catch((error) => {
            notifyError(error.message);
        });
        if (result) {
            setStateDate({ isLoading: false, data: result.data.items });
        }
    };
    const handleEdit = (n) => {
        props.history.push('/m/Settings/Roles/Update?r=' + (n.id ? n.id : ''));
    }
    useEffect(() => {
        fetchData();
    }, []);
    const notifyError = (msg) => toast(msg, {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'error'
    });
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
                                    <ReactTable
                                        columns={cols}
                                        defaultPageSize={10}
                                        className="-striped -highlight"
                                        manual // Forces table not to paginate or sort automatically, so we can handle it server-side
                                        data={stateData.data}
                                        pages={1} // Display the total number of pages
                                        loading={stateData.loading} // Display the loading overlay when we need it
                                        // onFetchData={this.fetchData} // Request new data when things change

                                        getTrProps={(stateData, row) => ({
                                            onClick: () => { handleEdit(row.original) }
                                        })}

                                    >
                                        {(stateData, makeTable, instance) => {
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
};

export default RoleList;