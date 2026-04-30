import { default as React, Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import Loader from 'react-loaders';
import 'react-table-6/react-table.css';
import { Slide, toast } from 'react-toastify';
import { Button, Card, CardBody, CardTitle, Col, CustomInput, FormGroup, Input, Row, Table } from 'reactstrap';
import * as formUtil from '../../../utils/form';
import SettingsService from '../../../services/Settings';

const queryString = require('query-string');


const Privilege_View = 1;
const Privilege_Add = 2;
const Privilege_Update = 4;
const Privilege_Delete = 8;

const AddOrEdit = (props) => {
    const parsed = queryString.parse(props.location.search);
    const [roleId, setRoleId] = useState('');
    const [stateData, setStateDate] = useState({ isLoading: true, data: null })
 
    //On route change update formid
    if (parsed.r !== roleId) {
        setRoleId(parsed.r);
    }

    const fetchData = async () => {

        if (roleId == null) {
            await SettingsService.getRolePages()
                .then((roleResponse) => {
                    let privileges = [];
                    if (Array.isArray(roleResponse.data)) {
                        privileges = roleResponse.data.map((item) => {
                            return { pageId: item.id, name: item.name, privilege: 0, module: item.module, displayName: item.displayName };
                        })
                    }
                    setStateDate({ isLoading: false, privileges: privileges, roleName: '', cards: [], canEdit: true });
                });
        } else {
            await SettingsService.getRolePagesById(roleId)
                .then((roleResponse) => { 
                    const cards = roleResponse.data.cards == null ? [] : roleResponse.data.cards.split(',');
                    setStateDate({ isLoading: false, privileges: roleResponse.data.privileges, 
                        roleName: roleResponse.data.name,  canEdit: roleResponse.data.canEdit, cards: cards });
                });
        }
    };

    useEffect(() => {
        if (roleId !== '') {
            fetchData();
        }

    }, []);

    const notifyError = (msg) => toast(msg, {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'error'
    });

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
        const sel = stateData.privileges.map((item, k) => {
            if (item.pageId === id) {
                item.privilege = (item.privilege & privilegeVal) === privilegeVal ? (item.privilege - privilegeVal) : (item.privilege + privilegeVal);
            }
            return item;
        });
        setStateDate({ ...stateData, privileges: sel });
    }

    const handleSave = async () => {
        try { 
            const data = { name: stateData.roleName, cards: stateData.cards.toString(), canEdit: stateData.canEdit};
            data.privileges = stateData.privileges.map((item, k) => {
                if (roleId) {
                    return { Privilege: item.privilege, pageId: item.pageId, id: item.id, roleId: roleId }
                }
                return { Privilege: item.privilege, pageId: item.pageId, id: item.id }
            })
            if (roleId) {
                data['id'] = roleId;
            }
            await SettingsService.updateAsync(data);
          
            notifySaved();
            props.history.push('/m/Settings/Role');
        }
        catch (error) {
            formUtil.displayFormikError(error);
        }
    }
    const checkCardCheckSts = (id) => {
        const idx = stateData.cards.findIndex(item => item == id);
        if (idx < 0) return false;
        else return true;
    }
    const handleCardCheckboxChange = (e, id,) => {
        const cards = stateData.cards;
        const idx = cards.findIndex(item => item == id);
        if (idx < 0) {
            cards.push(id);
        } else {
            cards.splice(idx, 1);
        }
        setStateDate({ ...stateData, cards: cards });
    }

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
                    <Row>
                        <Col md="12">
                            <Card className="main-card mb-2">
                                <CardBody>
                                    <CardTitle>Role Name</CardTitle>
                                    <Row>
                                        <Col md="12">
                                            <FormGroup check inline>
                                                <Input name='roleName' type="text" onChange={handleTextChange} value={stateData.roleName} />
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                    {/* <Row>
                                        <Col md="6" className="mt-3">
                                            <Table className="table-bordered">
                                                <tr>
                                                    <th> Dashboard </th>
                                                    <th> View </th>
                                                </tr>
                                                {CardTypes.map((item, k) => {
                                                    return <tr key={`trcrd${k}`}>
                                                        <td>
                                                            {item.name}
                                                        </td>
                                                        <td>
                                                            <FormGroup key={k} check inline>
                                                                <CustomInput key={`keycardchview${k}`} id={`idcardchview${k}`} name={`namecardchview${k}`}
                                                                    type="checkbox" onChange={(e) => handleCardCheckboxChange(e, item.id)}
                                                                    checked={checkCardCheckSts(item.id)} />View
                                                            </FormGroup>
                                                        </td>
                                                    </tr>
                                                })}
                                            </Table>
                                        </Col>
                                    </Row> */}
                                    <Row>
                                        <Col md="6" className="mt-3">
                                            <Table className="table-bordered">
                                                <tr>
                                                    <th>   </th>
                                                    <th> Page </th>
                                                    <th> View </th>
                                                    <th>  Add </th>
                                                    <th>  Update </th>
                                                    <th>  Delete  </th>
                                                </tr>
                                                {stateData.privileges && stateData.privileges.map((item, k) => {
                                                     
                                                    return <tr key={`tr${k}`}>
                                                         <td>
                                                            { k === 0 || stateData.privileges[k-1].module !== 
                                                            stateData.privileges[k].module ? item.module : ''}
                                                        </td>
                                                        <td>
                                                            {item.displayName}
                                                        </td>
                                                        <td>
                                                            <FormGroup key={k} check inline>
                                                                <CustomInput key={`keychview${k}`} id={`idchview${k}`} name={`namechview${k}`}
                                                                    type="checkbox" onChange={(e) => handleCheckboxChange(e, item.pageId, Privilege_View)}
                                                                    checked={(Privilege_View & item.privilege) === Privilege_View} />View
                                                        </FormGroup>
                                                        </td>
                                                        <td>
                                                            <FormGroup key={k} check inline>
                                                                <CustomInput key={`keychadd${k}`} id={`idchadd${k}`} name={`namechadd${k}`}
                                                                    type="checkbox" onChange={(e) => handleCheckboxChange(e, item.pageId, Privilege_Add)}
                                                                    checked={(Privilege_Add & item.privilege) === Privilege_Add} />Add
                                                        </FormGroup>
                                                        </td>
                                                        <td>
                                                            <FormGroup key={k} check inline>
                                                                <CustomInput key={`keychupdate${k}`} id={`idchupdate${k}`} name={`namechupdate${k}`}
                                                                    type="checkbox" onChange={(e) => handleCheckboxChange(e, item.pageId, Privilege_Update)}
                                                                    checked={(Privilege_Update & item.privilege) === Privilege_Update} />Update
                                                        </FormGroup>
                                                        </td>
                                                        <td>
                                                            <FormGroup key={k} check inline>
                                                                <CustomInput key={`keychdelete${k}`} id={`idchdelete${k}`} name={`namechdelete${k}`}
                                                                    type="checkbox" onChange={(e) => handleCheckboxChange(e, item.pageId, Privilege_Delete)}
                                                                    checked={(Privilege_Delete & item.privilege) === Privilege_Delete} />Delete
                                                        </FormGroup>
                                                        </td>
                                                    </tr>
                                                })}
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
                                    :''}
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </ReactCSSTransitionGroup>
            </Fragment>
    );
};

export default AddOrEdit;