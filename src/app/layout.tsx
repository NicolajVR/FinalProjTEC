import type { Metadata } from 'next'
import Providers from './components/Providers'
import { getServerSession } from 'next-auth'
import { CssBaseline } from '@mui/material'
import Header from './components/Header/page'
import Layout from './components/Layout/page'
import { Component } from 'react'


export const metadata: Metadata = {
  title: 'Dashboard',
  description: 'Admin panel',
}

export default  function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="en">
      <body>
        
        <Providers>
        <CssBaseline>
          <Header/>
          <Layout>
          {children}
          </Layout>
        </CssBaseline>
        </Providers>
        
        </body>
      
    </html>
  )
}
