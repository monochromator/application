<template>
  <div class="md-toolbar-row">
    <span class="md-title" style="flex: 1">{{$t("toolbar.title")}}</span>

    <span style="flex: 1">{{device}} TODO ADD STYLE</span>

    <md-menu>
      <md-button class="md-icon-button" md-menu-trigger>
        <md-icon>translate</md-icon>
      </md-button>

      <md-menu-content>
        <md-menu-item @click="changeLang('fr')">
          <country-flag country='fr'/>
          <span>{{$t("toolbar.lang.fr")}}</span>
        </md-menu-item>

        <md-menu-item @click="changeLang('en')">
          <country-flag country='us'/>
          <span>{{$t("toolbar.lang.en")}}</span>
        </md-menu-item>
      </md-menu-content>
    </md-menu>
  </div>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from "vue-property-decorator";

    /**
     * Supported languages
     */
    enum SupportedLanguages {
      Fr = "fr",
      En = "en"
    }

    /**
     * Data hold by toolbar component
     */
    interface ToolBarData {
      menuVisible: boolean;
    }

    /**
     * Main component
     */
    @Component
    export default class ToolBar extends Vue {
      @Prop({ required: true })
      public device: string;
       
      /**
       * Change application language
       *
       * @param lang New language
       */
      changeLang(lang: SupportedLanguages) {
        this.$root.$i18n.locale = lang;
      }
    }
</script>
