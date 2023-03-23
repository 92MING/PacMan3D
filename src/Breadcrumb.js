import React from 'react';
import './Breadcrumb.css';
import { Breadcrumb } from 'antd';
export default () => (
  <Breadcrumb
    routes={[
      {
        path: '/home-page',
        breadcrumbName: 'Home',
      },
      {
        path: '',
        breadcrumbName: 'Navigator',
        children: [
          {
            path: '',
            breadcrumbName: 'Start New Game',
          },
          {
            path: '',
            breadcrumbName: 'Create Map',
          },
          {
            path: '',
            breadcrumbName: 'Map Menu',
          },
          {
            path: '',
            breadcrumbName: 'Online Forum',
          },
        ],
      },
    ]}
  />
);