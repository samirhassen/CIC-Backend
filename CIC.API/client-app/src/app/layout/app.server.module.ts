import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { AppComponent } from '../app.component';
import { FlexLayoutServerModule } from '@angular/flex-layout/server';

@NgModule({
  imports: [
    ServerModule,
    FlexLayoutServerModule
  ],
  bootstrap: [AppComponent],
})
export class AppServerModule { }