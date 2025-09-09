import styled from 'styled-components';
import { Button } from 'antd';

const GridActionStyled = styled(Button)`
  margin: 2px 8px !important;
  color: ${(props) =>
    props.darkmode == 'dark' ? 'rgba(255,255,255,.85) !important;' : '#1455aa !important;'};
  border-color: #283747 !important;
`;

export default GridActionStyled;
