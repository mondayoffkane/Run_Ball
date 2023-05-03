Pod::Spec.new do |s|
  s.name                  = 'Amazon-SDK-Plugin'
  s.version               = '5.12.1'
  s.summary               = 'Unity wrapper for APS iOS SDK'
  s.homepage              = 'https://github.com/amazon/amazon-unity-sdk'
  s.license               = { :type => 'Amazon', :file => 'APS_IOS_SDK-4.4.1/LICENSE.txt' }
  s.author                = { 'Amazon' => 'aps-github@amazon.com' }
  s.ios.deployment_target = '12.5'
  s.source                = { :tag => "v#{s.version}" }
  s.source_files          = '*.{h,m,mm}'
  
  s.dependency 'AmazonPublisherServicesSDK'
  s.pod_target_xcconfig = {
    'ENABLE_BITCODE' => 'NO',
    'OTHER_CPLUSPLUSFLAGS' => '-fcxx-modules',
  }
end