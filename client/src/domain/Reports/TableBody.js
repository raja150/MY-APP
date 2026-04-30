import React from 'react';
import * as  date from 'utils/date'

export default function TableBody(props) {
    //Getting Data and time
    const getDate = (col, value, data) => {
        switch (col.type) {
            case "DateTime":
                return date.DisplayDateTime(value);
            case "Date":
                return date.getDate(value);
            case "TimeInHours":
                return date.toHoursAndMinutes(value);
            case "DateinHoursMins":

                return date.getFormattedDateTime(value, "timepicker");
            default:
                //in database we are saving number but we need to display text to user
                //set the data in JSON which data need to be display
                //Make sure that object in the array will contains only value and text
                //ex:[{value:0,text:'Test'}] 
                //Test wiil display to user
                if (data.length > 0) {
                    let obj = data.find(x => x.value == value)
                    return obj && obj['text'] ? obj['text'] : '-'
                }
                return value;
        }
    }

    const formatTdAlignData = (type) => {
        switch (type) {
            case "Int32":
                return "text-right";
            default:
                return "text-left";
        }
    }
    return (
        <tbody>
            {props.data && props.data.length > 0 ? props.data.map((row, i) => {
                return (
                    <tr key={i}>
                        <td className='text-center' key={`sno${i}`}>{i + 1}</td>
                        {props.columns.map((column, j) => (

                            <td className={`${formatTdAlignData(props.columns[j].type)}`} key={j}>
                                {column.hasPopUp && column.hasPopUp.length > 0 && row[column.validation] != null ?
                                    <a href="javscript:void()" onClick={() => props.handlePopUp(row, column, column.hasPopUp, props.popUpFilters)}>{getDate(props.columns[j], row[column.name], column.data)}</a> :
                                    getDate(props.columns[j], row[column.name], column.data)}
                            </td>
                        ))}
                    </tr>
                )
            }) :
                <tr>
                    <td style={{borderRight:'0'}}></td>
                    <td colSpan={props.columns.length} style={{ borderLeft:'0'}} className='text-center'>No Data Found
                    </td>
                </tr>
            }
        </tbody>
    )
}