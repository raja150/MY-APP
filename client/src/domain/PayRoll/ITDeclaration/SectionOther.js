import { faPlus, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RWDropdownList } from 'components/dynamicform/Controls';
import { Field } from 'formik';
import React, { Fragment } from 'react';
import { Button, Col, Label, Row } from 'reactstrap';
import { OtherEmptyItem } from './ConstValues';
import Error from '../../../components/dynamicform/Controls/Error';

const SectionOther = ({ form, remove, push, ...props }) => {
    const { values, errors, touched } = form;

    const handleValueChange = async (name, value, { selected, index }) => {
        form.setFieldValue(name, value);
        if (selected !== undefined) {
            form.setFieldValue(`sectionOtherLines.${index}.limit`, selected.limit)
        }
    }

    const lineItemErrorMsg = (error, name, i) => {
        if (error && error[i] && error[i][name]) {
            return error[i][name];
        }
        return "";
    }

    const lineItemIsTouched = (touch, name, i) => {
        if (touch && touch[i] && touch[i][name]) {
            return touch[i][name];
        }
        return false;
    }

    return (
        <Fragment>
            {values.sectionOtherLines.length > 0 &&
                values.sectionOtherLines.map((item, index) => {
                    return (
                        <div>
                            <Row>
                                <Col md='5'>
                                    <RWDropdownList
                                        {...{
                                            name: `sectionOtherLines.${index}.otherSectionsId`,
                                            label: 'Select An Investment',
                                            valueField: 'id',
                                            textField: 'name',
                                            index: index,
                                            value: item.otherSectionsId,
                                            values: values.sectionOthers,
                                            error: lineItemErrorMsg(errors.sectionOtherLines, 'otherSectionsId'),
                                            touched: lineItemIsTouched(touched.sectionOtherLines, 'otherSectionsId'),
                                        }}
                                        handlevaluechange={handleValueChange}
                                    />
                                    <span><strong>Max Limit : </strong> {item.limit}</span>

                                </Col>
                                <Col md='4'>
                                    <Label>Amount</Label>
                                    <Field
                                        type="number"
                                        name={`sectionOtherLines.${index}.amount`}
                                        className={`form-control form-control-sm ${lineItemIsTouched(errors.sectionOtherLines, 'amount', index) && lineItemErrorMsg(errors.sectionOtherLines, 'amount', index) ? 'is-invalid' : ''
                                            }`}
                                        value={item.amount}
                                    />
                                    <Error touched={touched} error={lineItemErrorMsg(errors.sectionOtherLines, 'amount', index)} />
                                </Col>
                           <Col md='3'>
                            {
                                values.sectionOtherLines.length > 1 ?
                                    <Row>
                                        <Col md='12' className='mt-3'>
                                            <Button
                                                className="p-1 border-0 btn-transition"
                                                outline
                                                tabIndex="-1"
                                                color="danger"
                                                name="Action"
                                                onClick={() => { remove(index) }}
                                            >   Remove Other Investment&nbsp;
                                                <FontAwesomeIcon icon={faTrashAlt} />
                                            </Button>
                                        </Col>
                                    </Row> : ''
                            }
                            </Col>
                             </Row>
                            <hr />
                        </div>
                    )
                })}
            {/* <tr>
                <th className="text-left">
                    Add Investments
                    <Button
                        onClick={() => {
                            push(OtherEmptyItem());
                        }}
                        className="p-1 border-0 btn-transition"
                        outline
                        color="primary"
                    >
                        <FontAwesomeIcon icon={faPlus} />
                    </Button>
                </th>
            </tr> */}
            <Button
                onClick={() => {
                    push(OtherEmptyItem());
                }}
                className="p-1 border-0 btn-transition"
                outline
                color="primary"
            >   Add Other Investments&nbsp;
                <FontAwesomeIcon icon={faPlus} />
            </Button>
        </Fragment>
    )
}
export default SectionOther;