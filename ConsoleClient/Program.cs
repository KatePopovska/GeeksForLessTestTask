﻿using DAL.Entities;
using BLL.Services;
using Newtonsoft.Json.Linq;
using BLL;

string jsonString = @"{
            'vm': {
                'ip': '192.168.44.44',
                'memory': '1024',
                'synced_folders': [
                    {
                        'host_path': 'data/',
                        'guest_path': '/var/www',
                        'type': 'default'
                    }
                ],
                'forwarded_ports': []
            },
            'vdd': {
                'sites': {
                    'drupal8': {
                        'account_name': 'root',
                        'account_pass': 'root',
                        'account_mail': 'box@example.com',
                        'site_name': 'Drupal 8',
                        'site_mail': 'box@example.com',
                        'vhost': {
                            'document_root': 'drupal8',
                            'url': 'drupal8.dev',
                            'alias': ['www.drupal8.dev']
                        }
                    },
                    'drupal7': {
                        'account_name': 'root',
                        'account_pass': 'root',
                        'account_mail': 'box@example.com',
                        'site_name': 'Drupal 7',
                        'site_mail': 'box@example.com',
                        'vhost': {
                            'document_root': 'drupal7',
                            'url': 'drupal7.dev',
                            'alias': ['www.drupal7.dev']
                        }
                    }
                }
            }
        }";

JToken jsonToken = JToken.Parse(jsonString);
ConfigItem rootConfigItem = ConfigParser.ParseJToken(jsonToken);

Console.ReadKey();
