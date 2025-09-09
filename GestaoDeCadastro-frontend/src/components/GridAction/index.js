/* eslint-disable react/jsx-props-no-spreading */
import React from 'react';
import PropTypes from 'prop-types';
import { Popconfirm } from 'antd';
import { useSelector } from 'react-redux';

import GridActionStyled from './GridAction.styled';
import { Link } from 'react-router-dom';

function GridAction({ item, actions }) {
  const darkmode = useSelector((state) => {
    return state.darkmode;
  });
  return actions.map((action) => {
    const showAction = action.show ? action.show(item) : true;

    const defaultProps = {
      key: action.title,
      type: action.type || 'primary',
      shape: 'circle',
      size: 'small',
      title: action.title,
      icon: action.icon,
      ghost: action.ghost === undefined || action.ghost === null ? true : action.ghost,
    };

    const navigateUrl = action.url ? action.url(item) : false;

    if (showAction) {
      if (navigateUrl) {
        return (
          <Link to={navigateUrl}>
            <GridActionStyled darkmode={darkmode} {...defaultProps} />
          </Link>
        );
      } else {
        return action.popConfirm ? (
          <Popconfirm
            key={action.title}
            title={action.popMsg ? action.popMsg : 'Tem certeza disso?'}
            cancelText="Não"
            okText="Sim"
            onConfirm={() => action.onClick(item)}
            okButtonProps={action.popConfirmDanger ? { danger: true } : null}
          >
            <GridActionStyled darkmode={darkmode} {...defaultProps} />
          </Popconfirm>
        ) : (
          <GridActionStyled
            darkmode={darkmode}
            {...defaultProps}
            onClick={() => action.onClick(item)}
          />
        );
      }
    }

    return null;
  });
}

GridAction.propTypes = {
  item: PropTypes.any.isRequired,
  actions: PropTypes.arrayOf(
    PropTypes.shape({
      title: PropTypes.string.isRequired,
      icon: PropTypes.any.isRequired,
      onClick: PropTypes.func.isRequired,
      ghost: PropTypes.bool,
    }),
  ).isRequired,
};

export default GridAction;
