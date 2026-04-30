import { faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { notifyError } from 'components/alert/Toast';
import * as _ from 'lodash';
import React, { Fragment, useState } from 'react';
import { AsyncTypeahead } from 'react-bootstrap-typeahead';
import { Button, Label } from 'reactstrap';
import SearchService from 'services/Search';
import { Details } from './ConstValues';

function MailsList({ form, push, remove, ...props }) {
    const { values,  setFieldValue } = form
    const [empS, setEmpS] = useState([])
    const [isSearching, setIsSearching] = useState(false);
    
    return (
        <Fragment>
            {values.mails.length > 0 &&
                values.mails.map((mail, index) => {
                    return (
                        <tr key={index + 1}>
                            <td >
                                <AsyncTypeahead
                                    name={`mails.${index}.id`}
                                    placeholder='Search with Employee Name'
                                    onChange={async (e) => {
                                        if (e.length > 0) {
                                            const selEmployee = e[0];
                                            const isExist = values.mails.find(e => e.id == selEmployee.id)
                                            
                                            if (!_.isEmpty(isExist)) {
                                                setFieldValue(`mails.${index}`, {})
                                                notifyError(selEmployee.name + " Already selected")
                                            } else {
                                                setFieldValue(`mails.${index}.id`, selEmployee.id)
                                                setFieldValue(`mails.${index}.name`, selEmployee.name)
                                                setFieldValue(`mails.${index}.workMail`, selEmployee.workEmail)
                                            }
                                        }
                                        else {
                                            setFieldValue(`mails.${index}`, {})
                                            setFieldValue(`mails.${index}.id`, '')
                                            setFieldValue(`mails.${index}.name`, '')
                                            setFieldValue(`mails.${index}.workMail`, '')
                                        }
                                    }}

                                    onSearch={async (query) => {
                                        setIsSearching(true);
                                        await SearchService.HelpDeskSearch(query,values.raiseById).then((result) => {
                                            setEmpS(result.data)
                                        })
                                        setIsSearching(false);
                                    }}

                                    renderMenuItemChildren={(option) => {
                                        return [
                                            <div>
                                                <div><strong>Name:{option.name}</strong></div>
                                                <div>No:{option.no}</div>
                                                <div>Mail:{option.workEmail}</div>
                                            </div>
                                        ]
                                    }}
                                    isLoading={isSearching}
                                    options={empS}
                                    labelKey={option => `${option.name}`}
                                    selected={mail && mail.id ? [mail] : []}
                                    id='id'
                                    clearButton
                                />
                            </td>
                            <td width={10}>
                                <Button onClick={() => { remove(index); }}
                                    className="p-1 border-0 btn-transition" outline color="danger">
                                    <FontAwesomeIcon icon={faTrash} />
                                </Button>
                            </td>
                        </tr>
                    )
                })
            }
            <tr>
                <td colSpan="4">
                    <Button onClick={() => { push(Details()); }}
                        className="p-1 border-0 btn-transition" outline color="primary">
                        <Label>Add Mail</Label> <FontAwesomeIcon icon={faPlus} ></FontAwesomeIcon>
                    </Button>
                </td>
            </tr>

        </Fragment>
    )
}
export default MailsList