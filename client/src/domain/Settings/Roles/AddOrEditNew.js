import Loading from 'components/Loader';
import { notifyError } from 'components/alert/Toast';
import { RWDropdownList } from 'components/dynamicform/Controls';
import React, { Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { Slide, toast } from 'react-toastify';
import { Button, Card, CardBody, CardTitle, Col, CustomInput, FormGroup, Input, Row, Table } from 'reactstrap';
import * as crypto from 'utils/Crypto';
import SettingsService from '../../../services/Settings';
import APIService from '../../../services/apiservice';
import * as formUtil from '../../../utils/form';

const queryString = require('query-string');
const Privilege_Read = 1;
const Privilege_Create = 2;
const Privilege_Update = 4;
const Privilege_Delete = 8;
const AddOrEditNew = (props) => {

    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [stateData, setStateDate] = useState({ isLoading: true, data: null, })
    const [state, setState] = useState({ modules: [], reports: [], selReports: [] });
    const [moduleId, setModuleId] = useState('')

    useEffect(() => {
        let modules = [];
        let reports = [], roleName = '', canEdit = true, cards = [], allPages = [];
        const fetchData = async () => {

            await APIService.getAsync('AppReport/Modules').then(response => {
                modules = response.data;
            }).catch(err => {
                notifyError(err.message);
            });

            await SettingsService.getPages().then((roleResponse) => {
                if (Array.isArray(roleResponse.data)) {
                    allPages = roleResponse.data.map((item) => {
                        return {
                            pageId: item.id,
                            name: item.name,
                            privilege: 0,
                            module: item.module,
                            displayName: item.displayName
                        };
                    })
                }
            });

            if (rid) {
                await SettingsService.getRolePagesById(rid)
                    .then((res) => {
                        allPages.forEach((p, i) => {
                            var page = res.data.pages.find(x => x.pageId == p.pageId)
                            if (page) {
                                allPages[i].privilege = page.privilege
                            } else {
                                allPages[i].privilege = 0
                            }
                        })
                        reports = res.data.reports
                        roleName = res.data.name
                        canEdit = res.data.canEdit
                        cards = res.data.cards == null ? [] : res.data.cards.split(',');
                    });
            }
            setStateDate({
                ...stateData, reports: reports, pages: allPages, roleName: roleName,
                canEdit: canEdit, cards: cards, isLoading: false
            })
            setState({ ...state, modules: modules })
        }
        fetchData()
    }, [])

    const notifySaved = () => toast("Data saved successfully!", {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'success'
    });

    const handleTextChange = (e) => {
        setStateDate({ ...stateData, [e.target.name]: e.target.value });
    }

    const handleCheckboxChange = (e, id, privilegeVal) => {
        const sel = stateData.pages.map((item, k) => {
            if (item.pageId === id) {
                item.privilege = (item.privilege & privilegeVal) === privilegeVal ? (item.privilege - privilegeVal)
                    : (item.privilege + privilegeVal);
            }
            return item;
        });
        setStateDate({ ...stateData, pages: sel });
    }
    const handleSelectAll = (isChecked, id) => {
        const sel = stateData.pages.map((item, k) => {
            if (item.pageId === id) {
                item.privilege = isChecked ? (Privilege_Read + Privilege_Create + Privilege_Update + Privilege_Delete) : 0;
            }
            return item;
        });
        setStateDate({ ...stateData, pages: sel });
    }
    const handleReportCheckboxChange = (e, id) => {
        let rpt = stateData.reports.find(x => x.reportId == id)
        if (!rpt) {
            stateData.reports.push({
                reportId: id,
                privilege: false,
            })
        }
        const sel = stateData.reports.map((item, k) => {
            if (item.reportId === id) {
                item.privilege = !item.privilege;
            }
            return item;

        });
        setStateDate({ ...stateData, reports: sel });
    }
    const handleValueChange = async (e, value) => {
        if (value) {
            await APIService.getAsync(`AppReport/Reports/${value}`).then(res => {
                setState({ ...state, reports: res.data })
            })
            setModuleId(value)
        }
        else {
            setModuleId(value)
        }
    }
    const handleSave = async () => {
        try {
            const data = { name: stateData.roleName, cards: stateData.cards.toString(), canEdit: stateData.canEdit };
            data.pages = stateData.pages.map((item, k) => {
                if (rid) {
                    return { Privilege: item.privilege, pageId: item.pageId, id: item.id, roleId: rid }
                }
                return { Privilege: item.privilege, pageId: item.pageId, id: item.id }
            })
            data.reports = stateData.reports.map((item, k) => {
                if (rid) {
                    return { Privilege: item.privilege, reportId: item.reportId, roleId: rid }
                }
                return { Privilege: item.privilege, reportId: item.reportId }
            })
            if (rid) {
                data['id'] = rid;
            }
            await SettingsService.updateAsync(data);

            notifySaved();
            props.history.push('/m/Settings/Role');
        }
        catch (error) {
            formUtil.displayErrors(error);
        }
    }
    return (
        stateData.isLoading ? <Loading /> :
            <Fragment>
                <ReactCSSTransitionGroup
                    component="div"
                    transitionName="TabsAnimation"
                    transitionAppear={true}
                    transitionAppearTimeout={0}
                    transitionEnter={false}
                    transitionLeave={false}
                    key='roles'>
                    <Row>
                        <Col md="12">
                            <Card className="main-card mb-2">
                                <CardBody>
                                    <CardTitle>Role Name</CardTitle>
                                    <Row>
                                        <Col md="12">
                                            <FormGroup >
                                                <Input name='roleName' type="text"
                                                    onChange={handleTextChange} placeholder="Enter role name" value={stateData.roleName} />
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col md="9" >
                                            <Table className="table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th>Module</th>
                                                        <th> Page </th>
                                                        <th> All </th>
                                                        <th> View </th>
                                                        <th> Add </th>
                                                        <th> Update </th>
                                                        <th> Delete </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    {stateData.pages && stateData.pages.map((item, k) => {
                                                        return <tr key={`tr${k}`}>
                                                            <td>
                                                                {k === 0 || stateData.pages[k - 1].module !==
                                                                    stateData.pages[k].module ? item.module : ''}
                                                            </td>
                                                            <td>
                                                                {item.displayName}
                                                            </td>
                                                            <td>
                                                                <FormGroup key={k} check inline>
                                                                    <CustomInput key={`keyChkAll${k}`} id={`idChAll${k}`} name={`nameChAll${k}`}
                                                                        type="checkbox" onChange={(e) => handleSelectAll(e.target.checked, item.pageId)
                                                                        }
                                                                    />All
                                                                </FormGroup>
                                                            </td>
                                                            <td>
                                                                <FormGroup key={k} check inline>
                                                                    <CustomInput key={`keyChView${k}`} id={`idChView${k}`} name={`nameChView${k}`}
                                                                        type="checkbox" onChange={(e) => handleCheckboxChange(e, item.pageId, Privilege_Read)}
                                                                        checked={(Privilege_Read & item.privilege) === Privilege_Read} />View
                                                                </FormGroup>
                                                            </td>
                                                            <td>
                                                                <FormGroup key={k} check inline>
                                                                    <CustomInput key={`keyChAdd${k}`} id={`idChAdd${k}`} name={`nameChAdd${k}`}
                                                                        type="checkbox" onChange={(e) => handleCheckboxChange(e, item.pageId, Privilege_Create)}
                                                                        checked={(Privilege_Create & item.privilege) === Privilege_Create} />Add
                                                                </FormGroup>
                                                            </td>
                                                            <td>
                                                                <FormGroup key={k} check inline>
                                                                    <CustomInput key={`keyChUpdate${k}`} id={`idChUpdate${k}`} name={`nameChUpdate${k}`}
                                                                        type="checkbox" onChange={(e) => handleCheckboxChange(e, item.pageId, Privilege_Update)}
                                                                        checked={(Privilege_Update & item.privilege) === Privilege_Update} />Update
                                                                </FormGroup>
                                                            </td>
                                                            <td>
                                                                <FormGroup key={k} check inline>
                                                                    <CustomInput key={`keyChDelete${k}`} id={`idChDelete${k}`} name={`nameChDelete${k}`}
                                                                        type="checkbox" onChange={(e) => handleCheckboxChange(e, item.pageId, Privilege_Delete)}
                                                                        checked={(Privilege_Delete & item.privilege) === Privilege_Delete} />Delete
                                                                </FormGroup>
                                                            </td>
                                                        </tr>
                                                    })}
                                                </tbody>
                                            </Table>

                                        </Col>
                                        <Col md="6">
                                            <h2>Reports</h2>
                                            <RWDropdownList {...{
                                                name: 'moduleId', label: 'Select Module', valueField: 'id', textField: 'name',
                                                value: moduleId, values: state.modules,

                                            }} handlevaluechange={handleValueChange}
                                            />
                                            <Table className="table-bordered">
                                                <thead>
                                                    <tr>
                                                        {/* <th>Module</th> */}
                                                        <th> Page </th>
                                                        <th> View </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    {state.reports && state.reports.map((item, k) => {
                                                        const currentReport = stateData.reports.find((ite, j) => ite.reportId == item.id
                                                        );

                                                        return <tr key={`tr${k}`}>
                                                            <td>
                                                                {item.label}
                                                            </td>
                                                            <td>
                                                                <FormGroup key={k} check inline>
                                                                    <CustomInput key={`keyChRptView${k}`} id={`idChRptView${k}`} name={`nameChRptView${k}`}
                                                                        type="checkbox" onChange={(e) => handleReportCheckboxChange(e, item.id)}
                                                                        checked={(currentReport && currentReport.privilege) || false} />View
                                                                </FormGroup>
                                                            </td>
                                                        </tr>
                                                    })}
                                                </tbody>
                                            </Table>
                                        </Col>
                                    </Row>
                                    {stateData.canEdit ?

                                        <Row>
                                            <Col md="12">
                                                <Button className="mb-2 mr-2 btn-icon float-left" key='submit' color="success" type="button" name="submit" onClick={handleSave}>
                                                    <i className="pe-7s-prev btn-icon-wrapper font-weight-bold text-black"> </i> Save
                                                </Button>
                                            </Col>
                                        </Row>
                                        : ''}
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </ReactCSSTransitionGroup>
            </Fragment>
    );
};

export default AddOrEditNew;