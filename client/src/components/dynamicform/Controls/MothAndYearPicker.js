import classNames from 'classnames';
import Moment from 'moment';
import PropTypes from 'prop-types';
import React from 'react';
import { DateTimePicker } from 'react-widgets';
import momentLocalizer from 'react-widgets-moment';
import 'react-widgets/dist/css/react-widgets.css';
import { FormGroup } from 'reactstrap';
Moment.locale('en');
momentLocalizer();
const MothAndYearPicker = (props) => {
    let { dpchange, handlevaluechange, value, label, touched, error, className, name, type, showDate, showTime,
        views, format, min, max, editFormat, ...rest } = props;

    const cssClass = classNames(
        "form-control p-0 ",
        {
            "is-invalid": touched && error
        },
        className
    );

    const handleChange = async (date) => {
        date = (date === null) ? {} : date
        if ((editFormat == "MM/YY" || editFormat == "MMM/YY") && props.dateType) {
            let dt;
            if (props.dateType === 'Start') {
                dt = Moment(date).startOf('month').format(Moment.HTML5_FMT.DATETIME_LOCAL_SECONDS)
            } else if (props.dateType === 'Start') {
                dt = Moment(date).endOf('month').format(Moment.HTML5_FMT.DATETIME_LOCAL_SECONDS)
            } else {
                dt = Moment(date).format(Moment.HTML5_FMT.DATE);
            }
            handlevaluechange(props.name, Moment(date).format(Moment.HTML5_FMT.DATE));
        }
        else {
            handlevaluechange(props.name,
                showTime === 1 || showTime === true ? Moment(date).format(Moment.HTML5_FMT.DATETIME_LOCAL_SECONDS) : Moment(date).format(Moment.HTML5_FMT.DATE)
                , {});
        }
    }
    // if (format == undefined) {
    //     if (showDate === true && showTime === true) {
    //         format = "DD/MM/yyyy hh:mm";
    //     } else if (showDate === true) {
    //         format = "DD/MM/yyyy"
    //         // const date = new Date(Date.UTC(2020, 11, 20, 3, 23, 16, 738));
    //         // console.log(new Intl.DateTimeFormat('en-GB', { dateStyle: 'full', timeStyle: 'long' }).format(date));
    //     } else {
    //         format = "hh:mm A"
    //     }
    // }

    return (
        <FormGroup >
            {label ? <label className="text-capitalize" htmlFor={props.name}>{label}</label> : ''}

            <DateTimePicker
                name={props.name}
                value={value ? new Date(value) : null}
                defaultValue={value ? new Date(value) : null}
                onChange={handleChange}
                views={views ? views : ["month", "year", "decade", "century"]}
                // format={format}
                // min={min ? min : new Date(1900, 1, 1)}
                min={min ? min : new Date(1900, 1, 1)}
                max={max ? max : new Date(2100, 1, 1)}
                editFormat={editFormat ? editFormat : ""}
                className={cssClass}
                date={true}
                time={false}
                disabled={props.disabled}
                {...rest}
            />


            {touched && error ?
                <div className="invalid-feedback">
                    {error}
                </div>
                : ''}
        </FormGroup>
    );
};

MothAndYearPicker.propTypes = {
    name: PropTypes.string.isRequired,
};
export default MothAndYearPicker;
