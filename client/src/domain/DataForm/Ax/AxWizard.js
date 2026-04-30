import DynamicForm from 'components/dynamicform';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import React, { Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { Slide, toast } from 'react-toastify';
import { Button, CardTitle, Col, Container, Row } from 'reactstrap';
import APIService from 'services/apiservice';
import * as compare from 'utils/Compare';
import * as formUtil from 'utils/form';
import { Breadcrumb, BreadcrumbItem } from 'reactstrap';
import { useHistory } from 'react-router-dom';

export default function AxWizard(props) {
    const [initialValues, setIntValues] = useState({});
    const [validationSchema, setVldValues] = useState({});
    const [state, setState] = useState({ isLoading: true });
    const [wizardId, setWizardState] = useState(0);
    const [dpValues, setDropDownValues] = useState();
    const { module, displayName, editEntityId, refEntityId, tabJson, handleFromSaved } = props;

    const history = useHistory()
    useEffect(() => {
        const fetchData = async () => {
            let formData = {};
            if (editEntityId) {
                await APIService.getAsync(`${module}/${tabJson.name}/${editEntityId}`)
                    .then((response) => {
                        formData = response.data;
                    });
            }

            const fields = formUtil.getFieldsFromFieldGroup(tabJson);
            const yepSchema = formUtil.getYepSchema(fields);
            const iniValues = formUtil.getInitialValues(formData, fields);

            const ddData = await formUtil.getFromFieldData(fields, iniValues);
            //Update refEntityId as reference value to the tab fields
            if (tabJson.parent && refEntityId) {
                iniValues[tabJson.parent.refColumn] = refEntityId;
            }
            setIntValues(iniValues);
            setVldValues(yepSchema);
            setDropDownValues(ddData);
            setState({ isLoading: false, formGroups: tabJson.wizards.length, lookUpData: ddData, fields: fields });
        };

        fetchData();
    }, []);



    const notifySaved = () => toast("Data saved successfully!", {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'success'
    });
    const notifyError = (msg) => toast(msg, {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'error'
    });
    const saveData = async (values, actions) => {
        try {
            if (editEntityId) {
                await APIService.putAsync(`${module}/${tabJson.name}`, values);
                handleFromSaved(editEntityId);
                notifySaved();
            } else {
                await APIService.postAsync(`${module}/${tabJson.name}`, values).then((response) => {
                    handleFromSaved(response.data.id);
                    notifySaved();
                });
            }
        }
        catch (error) {
            formUtil.displayFormikError(error, actions);
        }

        actions.setSubmitting(false);
    };

    const nextClick = () => {
        if (state.formGroups > wizardId) {
            setWizardState(wizardId + 1);
        }
    };

    const previousClick = () => {
        if (state.formGroups > wizardId) {
            setWizardState(wizardId - 1);
        }
    };

    const renderSteps = () => {
        //When page has only one wizard then hide steps
        return compare.isEqual(tabJson.wizards.length, 1) ? '' :
            tabJson.wizards.map((s, i) => (
                <li
                    className={`form-wizard-step-${compare.isEqual(wizardId, i) ? 'doing' : wizardId > i ? 'done' : 'todo'}`}
                    onClick={() => { setWizardState(i) }}
                    key={i}
                    value={i}
                >
                    <em>{i + 1}</em>
                    <span>{s.displayName}</span>
                </li>

            ))
    };

    return (
        <Fragment>
            {state.isLoading ? (
                <Loading />
            ) :
                <ReactCSSTransitionGroup
                    component="div"
                    transitionName="TabsAnimation"
                    transitionAppear={true}
                    transitionAppearTimeout={0}
                    transitionEnter={false}
                    transitionLeave={false}>
                    {/* This displays when only wizard displays, when it display with tab not required */}
                    {
                        displayName ? <div className="app-page-title pt-2 pb-2">
                            <div className="page-title-wrapper">
                                <div className="page-title-heading">
                                    <Breadcrumb>
                                        <div style={{ fontSize: '30px' }} className={props.icon} />
                                        <BreadcrumbItem> {props.module} </BreadcrumbItem>
                                        <BreadcrumbItem>
                                            {/* <Link to={`/d/list/${fid}`}> */}
                                            {displayName}
                                            {/* </Link> */}
                                        </BreadcrumbItem>
                                        <BreadcrumbItem>
                                            {editEntityId ? "Update" : "New"}
                                        </BreadcrumbItem>
                                    </Breadcrumb>
                                </div>
                            </div>
                        </div> : ""
                    }

                    <Container fluid>
                        <Row>
                            <Col md="12">
                                <Formik enableReinitialize={true} key={'dFormik'}
                                    {...{ initialValues, validationSchema }}
                                    onSubmit={(values, actions, setSubmitting) => {
                                        // let formData = new FormData();
                                        // for (var x in values) {
                                        //     formData.append(x, values[x]);
                                        // } 
                                        saveData(values, actions);
                                    }}
                                >
                                    {({ handleSubmit, validateForm, setFieldTouched, isSubmitting, ...formikBag }) => (

                                        //Here onSubmit is to stop html form submit 
                                        <Form onSubmit={handleSubmit}>
                                            {
                                                compare.isEqual(tabJson.fieldGroupType ? tabJson.fieldGroupType.toLowerCase() : "", 'accordion') ?
                                                    <Fragment>
                                                        {tabJson.wizards.length > 1 ?
                                                            tabJson.wizards.map((group, k) => {
                                                                return <div key={`card${k}`} className="mb-3">
                                                                    <CardTitle>{group.displayName}</CardTitle>
                                                                    <DynamicForm accordion={true} key='form_dynamic'
                                                                        json={group} lookUpData={dpValues}  {...formikBag} />
                                                                </div>
                                                            })
                                                            :
                                                            <DynamicForm accordion={true} key='form_dynamic'
                                                                json={tabJson.wizards[0]} lookUpData={dpValues}  {...formikBag} />
                                                        }
                                                        {/* Cancel button show when the secondary tabs has multiple records are allowed */}
                                                        {props.handleCancel ? <Button className="mb-2 mr-2 " key='buttonCancel' color="secondary"
                                                            type="button" name="submit" onClick={props.handleCancel} >
                                                            Cancel
                                                        </Button> : ''}

                                                        <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonSubmit' color="success"
                                                            type="button" name="submit" onClick={handleSubmit} disabled={isSubmitting}>
                                                            <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i>
                                                            {isSubmitting ? "Please wait..." : "Submit"}
                                                        </Button>
                                                    </Fragment>
                                                    :
                                                    <Fragment>
                                                        <div className="forms-wizard-alt">
                                                            <ol className='forms-wizard'>
                                                                {renderSteps()}
                                                            </ol>
                                                        </div>
                                                        <DynamicForm accordion={false} key='form_dynamic'
                                                            json={tabJson.wizards[wizardId]} lookUpData={dpValues}  {...formikBag} />

                                                        {/* Cancel button show when the secondary tabs has multiple records are allowed */}
                                                        {props.handleCancel ? <Button className="mb-2 mr-2 " key='buttonCancel' color="secondary"
                                                            type="button" name="cancel" onClick={props.handleCancel} >
                                                            Cancel
                                                        </Button> : ''}

                                                        {compare.isEqual(wizardId, (state.formGroups - 1)) ?
                                                            <Row className='justify-content-center'>
                                                                <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonSubmit' color="success" type="button" name="submit" onClick={handleSubmit} disabled={isSubmitting}>
                                                                    <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {isSubmitting ? "Please wait..." : "Submit"}
                                                                </Button>
                                                                <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonCancel' color="success" type="button" name="cancel" onClick={() => history.goBack()} disabled={isSubmitting}>
                                                                    <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {isSubmitting ? "Please wait..." : "Go to list"}
                                                                </Button>
                                                            </Row>
                                                            :
                                                            <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonSubmit' color="success" type="button" name="submit" onClick={async () => {
                                                                const curErrors = await validateForm(); //Validate forms
                                                                let isValid = true; //To know form has errors or not
                                                                //map to each field from json to update filed is touched
                                                                tabJson.wizards[wizardId].fields.map((f, k) => {
                                                                    //Update touched status
                                                                    setFieldTouched(f.name);
                                                                    //Verify current map field is having error 
                                                                    if (curErrors[f.name] && isValid) {
                                                                        isValid = false;
                                                                    }
                                                                });
                                                                //all fields are valid then move to next
                                                                if (isValid) {
                                                                    nextClick();
                                                                }

                                                            }}> <i className="pe-7s-next btn-icon-wrapper font-weight-bold text-black"> </i> NEXT
                                                            </Button>
                                                        }
                                                        {
                                                            compare.isEqual(wizardId, 0) ? '' :
                                                                <Button className="mb-2 mr-2 btn-icon float-left" key='submitPrevious' color="warning" type="button" name="submit" onClick={previousClick}>
                                                                    <i className="pe-7s-prev btn-icon-wrapper font-weight-bold text-black"> </i> PREVIOUS
                                                                </Button>
                                                        }
                                                    </Fragment>
                                            }
                                        </Form>
                                    )}
                                </Formik>
                            </Col>
                        </Row>
                    </Container>
                </ReactCSSTransitionGroup>
            }
        </Fragment>
    )
}


// AxWizard.propTypes = {
//     entityData: PropTypes.object.isRequired,
//     entityId: PropTypes.string.isRequired,
//     handleFromSaved: PropTypes.func.isRequired
// };