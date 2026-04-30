import { notifyError } from 'components/alert/Toast';
import { RWDropdownList } from 'components/dynamicform/Controls';
import * as _ from 'lodash';
import React, { Fragment, useEffect, useState } from 'react';
import ApplyLeavesService from 'services/Leave/ApplyLeave';
import * as dateUtil from 'utils/date';
import Details from './ConstValues';

function SelectedLeaveTypes({ form, push, remove, ...props }) {
    const { values, errors, touched, setFieldValue } = form
    const [state, setState] = useState({ leaveType: [] });
    useEffect(() => {
        const FetchData = async () => {
            let leaveType = [];
            await ApplyLeavesService.GetEmployeeLeaveType().then((response) => {
                leaveType = response.data
            })
            setState({ ...state, leaveType: leaveType })
        }
        FetchData();
    }, [])

    const handleValueChange = async (name, value, selected, index) => {

        if(values.fromDate =='' || values.toDate ==''){
            return notifyError("Please select Dates")
        }
        if (value != "") {
            setFieldValue(name, value)
            let leaveBalance = '', leaveTypeList = []
            await ApplyLeavesService.GetLeaveBalanceByLeaveType(value,values.fromDate,values.toDate).then((res) => {
                leaveBalance = res.data
                 if (leaveBalance > 0) {
                    var booked = _.sumBy(values.applyLeaveType, function (o) { return o.noOfLeaves; })
                    var alreadyBooked = values.applyLeaveType.find(o => o.leaveTypeId === value)
                    var selectedDays = dateUtil.getDaysDifference(values.fromDate, values.toDate, values.fromHalfDisplay, values.toHalfDisplay);
                    if (_.isEmpty(alreadyBooked)) {
                        leaveTypeList.push({
                            leaveTypeId: value,
                            leaveTypeName: selected.leaveTypeName,
                            availableBalance: leaveBalance,
                            noOfLeaves: res.data - (selectedDays - booked) < 0 ? res.data : (selectedDays - booked),
                            orderBy: 0
                        });
                    }
                    else {
                        //remove when same LeaveType more then one selected
                        setFieldValue(name, '')
                        values.applyLeaveType.splice(index, 1)
                        notifyError('Leave type already selected!')
                    }
                    setFieldValue('applyLeaveType', values.applyLeaveType.concat(leaveTypeList))
                    //to create new dropdown verifying the condition i.e balance is negative or not
                    // and if balance is negative if  not push the new dropdown when it is already exist
                    var isNegative = res.data - (selectedDays - booked);
                    const exist = values.applyLeaveType.find(o => o.leaveTypeId === value);
                    isNegative < 0 && _.isEmpty(exist) && push(Details());
                }
                else{
                    setFieldValue(name, '')
                    notifyError(`Oops! Sorry, You don't have ${selected.leaveTypeName} balance`)
                }
            })
        }
        else {//if we click on dropdown (X) that should be remove
            setFieldValue(name, '')
            values.applyLeaveType.splice(index, 1)
        }
    }
    return (
        <Fragment>
            {values.selectedLeaveTypes.length > 0 &&
                values.selectedLeaveTypes.map((leaveType, index) => {
                    const leave = values.applyLeaveType.find(o => o.leaveTypeId === leaveType.leaveTypeId);
                    return (
                        <>
                            <RWDropdownList {...{
                                name: `selectedLeaveTypes.${index}.leaveTypeId`, label: '', valueField: 'leaveTypeId',
                                textField: 'leaveTypeName',
                                values: state.leaveType,
                                value: leaveType.leaveTypeId,
                                disabled: !_.isEmpty(leave) && leave.id || leave && leave.leaveTypeId && (leave.availableBalance - leave.noOfLeaves) >= 0 ? true : false,
                                error: errors[`selectedLeaveTypes.${index}.leaveTypeId`], touched: touched[`selectedLeaveTypes.${index}.leaveTypeId`],
                            }} handlevaluechange={(e, v, selected) => {
                                handleValueChange(e, v, selected.selected, index)
                            }
                            } />
                        </>
                    )
                })}

        </Fragment>
    )
}
export default SelectedLeaveTypes;